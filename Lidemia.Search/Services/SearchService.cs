// Created on 05/10/2026 by Laserson

using Lidemia.Core.Models.Configuration;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Misc;
using Lidemia.Search.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenSearch.Client;
using OpenSearch.Net;
using Scrutor;

namespace Lidemia.Search.Services;

[ServiceDescriptor<ISearchService>(ServiceLifetime.Singleton)]
public class SearchService : ISearchService
{ private readonly IOpenSearchClient client;

    private readonly OpenSearchSettings settings;

    public SearchService(IOpenSearchClient client, IOptions<OpenSearchSettings> settings)
    {
        this.client = client;
        this.settings = settings.Value;
    }

    public async Task<IEnumerable<ProductMainInfoModel>> Search(ProductsListFilterModel filter, CancellationToken cancellationToken = default)
    {
        var queries = new List<QueryContainer>
        {
            Query<ProductMainInfoModel>.Term(term => term
                .Field(field => field.IsVisible)
                .Value(true))
        };

        if (!string.IsNullOrWhiteSpace(filter.Title))
        {
            queries.Add(Query<ProductMainInfoModel>
                .Wildcard(match => match
                    .Field(field => field.Title)
                    .Value($"*{filter.Title}*")
                    .CaseInsensitive()));
        }

        if (filter.Tags?.Length > 0)
        {
            queries.Add(Query<ProductMainInfoModel>.Terms(x => x.Field(f => f.Tags).Terms(filter.Tags)));
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
        int from = (page - 1) * filter.PageSize;

        ISearchResponse<ProductMainInfoModel> response = await this.client.SearchAsync<ProductMainInfoModel>(search => search
            .Index(this.settings.IndexName)
            .From(from)
            .Size(filter.PageSize)
            .Query(_ => query), cancellationToken);

        return response.Documents;
    }

    public async Task<bool> UpdateInIndex(ProductMainInfoModel model, CancellationToken cancellationToken = default)
    {
        IndexResponse response = await this.client.IndexAsync(model, index => index
            .Index(this.settings.IndexName)
            .Id(model.Id)
            .Refresh(Refresh.WaitFor), cancellationToken);

        return response.IsValid;
    }

    public async Task<bool> RemoveFromIndex(long id, CancellationToken cancellationToken = default)
    {
        DeleteResponse response = await this.client.DeleteAsync<ProductMainInfoModel>(id, delete => delete
            .Index(this.settings.IndexName)
            .Refresh(Refresh.WaitFor), cancellationToken);

        return response.IsValid;
    }

    public async Task<bool> RebuildIndex(IEnumerable<ProductMainInfoModel> products, CancellationToken cancellationToken = default)
    {
        await this.DeleteIndex(cancellationToken);
        await this.CreateIndex(cancellationToken);

        BulkResponse response = await this.client.BulkAsync(bulk => bulk
            .Index(this.settings.IndexName)
            .IndexMany(products, (descriptor, product) => descriptor.Id(product.Id))
            .Refresh(Refresh.WaitFor), cancellationToken);

        return response.IsValid && !response.Errors;
    }

    private async Task DeleteIndex(CancellationToken cancellationToken)
    {
        ExistsResponse exists = await this.client.Indices.ExistsAsync(this.settings.IndexName, ct: cancellationToken);

        if (exists.Exists)
        {
            await this.client.Indices.DeleteAsync(this.settings.IndexName, ct: cancellationToken);
        }
    }

    private Task<CreateIndexResponse> CreateIndex(CancellationToken cancellationToken)
    {
        return this.client.Indices.CreateAsync(this.settings.IndexName, create => create
                .Settings(s =>
                {
                    return s.Analysis(a => a.Tokenizers(t => t.NGram("title_ngram_tokenizer", ng => ng
                        .MinGram(2)
                        .MaxGram(10)
                        .TokenChars(
                            TokenChar.Letter,
                            TokenChar.Digit
                        ))).Analyzers(an => an
                        .Custom("title_substring_analyzer", ca => ca.Tokenizer("title_ngram_tokenizer").Filters("lowercase")
                        )
                    )).Analysis(a => a.Tokenizers(t => t.NGram("description_ngram_tokenizer", ng => ng
                        .MinGram(2)
                        .MaxGram(10)
                        .TokenChars(
                            TokenChar.Letter,
                            TokenChar.Digit
                        ))).Analyzers(an =>
                        an.Custom("description_substring_analyzer", ca => ca.Tokenizer("description_ngram_tokenizer").Filters("lowercase"))));
                })
                .Map<ProductMainInfoModel>(map => map
                    .Properties(properties => properties
                        .Number(x => x.Name(field => field.Id).Type(NumberType.Long))
                        .Text(x => x.Name(field => field.Title).Analyzer("title_substring_analyzer").SearchAnalyzer("standard"))
                        .Text(x => x.Name(field => field.Description).Analyzer("description_substring_analyzer").SearchAnalyzer("standard"))
                        .Text(x => x.Name(f => f.Tags))
                        .Number(x => x.Name(f => f.CategoryId))
                        .Number(x => x.Name(field => field.Price).Type(NumberType.Double))
                        .Boolean(x => x.Name(field => field.IsVisible)))),
            cancellationToken);
    }
}