// Created on 05/10/2026 by Laserson

using Lidemia.Core.Models.Configuration;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Misc;
using Lidemia.Search.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenSearch.Client;
using OpenSearch.Net;
using Scrutor;

namespace Lidemia.Search.Services;

[ServiceDescriptor<ISearchService>(ServiceLifetime.Singleton)]
public class SearchService : ISearchService
{
    private const int MinGram = 2;
    private const int MaxGram = 20;

    private const int DefaultPageSize = 20;
    private const int MaxPageSize = 100;

    private const string NgramTokenizerName = "products_ngram_tokenizer";
    private const string SubstringAnalyzerName = "products_substring_analyzer";
    private const string SearchAnalyzerName = "products_search_analyzer";
    private const string LowercaseNormalizerName = "products_lowercase_normalizer";
    private const string KeywordSubFieldName = "keyword";
    private const string AlreadyExistsErrorType = "resource_already_exists_exception";

    private readonly IOpenSearchClient client;

    private readonly OpenSearchSettings settings;

    private readonly ILogger<SearchService> logger;

    public SearchService(IOpenSearchClient client, IOptions<OpenSearchSettings> settings, ILogger<SearchService> logger)
    {
        this.client = client;
        this.settings = settings.Value;
        this.logger = logger;
    }

    public async Task<IEnumerable<ProductMainInfoModel>> Search(ProductsListFilterModel filter, CancellationToken cancellationToken = default)
    {
        List<QueryContainer> queries =
        [
            Query<ProductMainInfoModel>.Term(term => term
                .Field(field => field.IsVisible)
                .Value(true))
        ];

        if (!string.IsNullOrWhiteSpace(filter.Title))
        {
            // The title field is indexed with an ngram analyzer, so a "match" query covers partial
            // (substring) input in any case. A wildcard query cannot do either: it never matches
            // analyzed ngram terms that contain a space and its "case_insensitive" flag only works
            // for ASCII characters (no effect on Cyrillic).
            queries.Add(Query<ProductMainInfoModel>.MultiMatch(match => match
                .Fields(fields => fields.Field(field => field.Title, 2))
                .Query(filter.Title.Trim())
                .Operator(Operator.And)));
        }

        if (!string.IsNullOrEmpty(filter.Tag))
        {
            // Tags are stored as analyzed text, so the exact-match filter must use the
            // lowercased "keyword" sub-field instead of the analyzed field itself.
            queries.Add(Query<ProductMainInfoModel>.Term(term => term
                .Field(field => field.Tags.Suffix(KeywordSubFieldName))
                .Value(filter.Tag.Trim().ToLowerInvariant())));
        }

        if (filter.PriceFrom.HasValue || filter.PriceTo.HasValue)
        {
            queries.Add(Query<ProductMainInfoModel>.Range(range =>
            {
                range = range.Field(field => field.Price);

                if (filter.PriceFrom.HasValue)
                {
                    range = range.GreaterThanOrEquals((double)filter.PriceFrom.Value);
                }

                if (filter.PriceTo.HasValue)
                {
                    range = range.LessThanOrEquals((double)filter.PriceTo.Value);
                }

                return range;
            }));
        }

        QueryContainer query = queries.Count == 0
            ? new MatchAllQuery()
            : new BoolQuery
            {
                Must = queries
            };

        int page = filter.Page < 1 ? 1 : filter.Page;
        int pageSize = filter.PageSize is < 1 or > MaxPageSize ? DefaultPageSize : filter.PageSize;
        int from = (page - 1) * pageSize;

        var response = await this.client.SearchAsync<ProductMainInfoModel>(search => search
            .Index(this.settings.IndexName)
            .From(from)
            .Size(pageSize)
            .Query(_ => query), cancellationToken);

        if (!response.IsValid)
        {
            string reason = DescribeError(response);

            this.logger.LogError("Products search failed: {Reason}", reason);
            throw new InvalidOperationException($"Products search failed: {reason}");
        }

        return response.Documents;
    }

    public async Task<bool> UpdateInIndex(ProductMainInfoModel model, CancellationToken cancellationToken = default)
    {
        await this.EnsureIndexExists(cancellationToken);

        IndexResponse response = await this.client.IndexAsync(model, index => index
            .Index(this.settings.IndexName)
            .Id(model.Id)
            .Refresh(Refresh.WaitFor), cancellationToken);

        if (!response.IsValid)
        {
            this.logger.LogError("Failed to index product {Id}: {Reason}", model.Id, DescribeError(response));
            return false;
        }

        return true;
    }

    public async Task<bool> RemoveFromIndex(long id, CancellationToken cancellationToken = default)
    {
        DeleteResponse response = await this.client.DeleteAsync<ProductMainInfoModel>(id, delete => delete
            .Index(this.settings.IndexName)
            .Refresh(Refresh.WaitFor), cancellationToken);

        if (!response.IsValid)
        {
            this.logger.LogError("Failed to remove product {Id} from the index: {Reason}", id, DescribeError(response));
            return false;
        }

        return true;
    }

    public async Task<bool> RebuildIndex(IEnumerable<ProductMainInfoModel> products, CancellationToken cancellationToken = default)
    {
        await this.DeleteIndex(cancellationToken);

        CreateIndexResponse createResponse = await this.CreateIndex(cancellationToken);

        // Without this check a failed index creation is silently ignored and the first indexed
        // document auto-creates the index with a dynamic mapping, which breaks substring search.
        if (!createResponse.IsValid)
        {
            this.logger.LogError("Failed to create search index {Index}: {Reason}", this.settings.IndexName, DescribeError(createResponse));
            return false;
        }

        BulkResponse response = await this.client.BulkAsync(bulk => bulk
            .Index(this.settings.IndexName)
            .IndexMany(products, (descriptor, product) => descriptor.Id(product.Id))
            .Refresh(Refresh.WaitFor), cancellationToken);

        if (!response.IsValid || response.Errors)
        {
            this.logger.LogError("Failed to populate search index {Index}: {Reason}", this.settings.IndexName, DescribeError(response));
            return false;
        }

        return true;
    }

    private async Task DeleteIndex(CancellationToken cancellationToken)
    {
        ExistsResponse exists = await this.client.Indices.ExistsAsync(this.settings.IndexName, ct: cancellationToken);

        if (exists.Exists)
        {
            await this.client.Indices.DeleteAsync(this.settings.IndexName, ct: cancellationToken);
        }
    }

    /// <summary>
    /// Indexing into a missing index makes OpenSearch auto-create it with a dynamic mapping,
    /// which breaks substring search, so the index is re-created explicitly when it is gone.
    /// </summary>
    private async Task EnsureIndexExists(CancellationToken cancellationToken)
    {
        ExistsResponse exists = await this.client.Indices.ExistsAsync(this.settings.IndexName, ct: cancellationToken);

        if (exists.Exists)
        {
            return;
        }

        CreateIndexResponse createResponse = await this.CreateIndex(cancellationToken);

        // Another instance may have created the index in the meantime.
        if (createResponse.IsValid || createResponse.ServerError?.Error?.Type == AlreadyExistsErrorType)
        {
            return;
        }

        string reason = DescribeError(createResponse);

        this.logger.LogError("Failed to create search index {Index}: {Reason}", this.settings.IndexName, reason);
        throw new InvalidOperationException($"Failed to create search index {this.settings.IndexName}: {reason}");
    }

    private Task<CreateIndexResponse> CreateIndex(CancellationToken cancellationToken)
    {
        return this.client.Indices.CreateAsync(this.settings.IndexName, create => create
                .Settings(settings => settings
                    // OpenSearch rejects an ngram tokenizer whose min/max gram difference exceeds
                    // "index.max_ngram_diff" (default 1), which silently broke the index before.
                    .Setting("index.max_ngram_diff", MaxGram - MinGram)
                    .Analysis(analysis => analysis
                        .Normalizers(normalizers => normalizers
                            .Custom(LowercaseNormalizerName, normalizer => normalizer.Filters("lowercase")))
                        .Tokenizers(tokenizers => tokenizers
                            .NGram(NgramTokenizerName, tokenizer => tokenizer
                                .MinGram(MinGram)
                                .MaxGram(MaxGram)
                                .TokenChars(
                                    TokenChar.Letter,
                                    TokenChar.Digit)))
                        .Analyzers(analyzers => analyzers
                            .Custom(SubstringAnalyzerName, analyzer => analyzer
                                .Tokenizer(NgramTokenizerName)
                                .Filters("lowercase"))
                            // The search analyzer must be lowercase-only: ngrams are only applied at
                            // index time, so queries are matched against them case-insensitively.
                            .Custom(SearchAnalyzerName, analyzer => analyzer
                                .Tokenizer("standard")
                                .Filters("lowercase")))))
                .Map<ProductMainInfoModel>(map => map
                    .Properties(properties => properties
                        .Number(x => x.Name(field => field.Id).Type(NumberType.Long))
                        .Text(x => x.Name(field => field.Title).Analyzer(SubstringAnalyzerName).SearchAnalyzer(SearchAnalyzerName))
                        .Text(x => x.Name(field => field.Description).Analyzer(SubstringAnalyzerName).SearchAnalyzer(SearchAnalyzerName))
                        .Text(x => x.Name(f => f.Tags).Fields(fields => fields
                            .Keyword(keyword => keyword.Name(KeywordSubFieldName).Normalizer(LowercaseNormalizerName))))
                        .Number(x => x.Name(field => field.SupplierId).Type(NumberType.Long))
                        .Number(x => x.Name(f => f.CategoryId).Type(NumberType.Long))
                        .Number(x => x.Name(field => field.Price).Type(NumberType.Double))
                        .Boolean(x => x.Name(field => field.IsVisible)))),
            cancellationToken);
    }

    private static string DescribeError(IResponse response)
    {
        return response.ServerError?.Error?.Reason ?? response.OriginalException?.Message ?? "unknown error";
    }
}