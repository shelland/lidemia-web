// Created on 02/11/2026 19:54 by Laserson

using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Service;
using Lidemia.Search.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers;

[Route("Search")]
public class SearchController : BaseController
{
    private readonly ISearchService searchService;

    public SearchController(ISearchService searchService)
    {
        this.searchService = searchService;
    }

    [HttpGet("Products/Autocomplete")]
    public async Task<ResultInfo<ProductSearchAutocompleteResponseDto>> ProductsAutocomplete(
        [FromQuery] ProductSearchAutocompleteRequestDto request,
        CancellationToken cancellationToken)
    {
        var results = await this.searchService.Search(new ProductsListFilterModel
        {
            Title = request.Query,
            PageSize = request.Limit
        }, cancellationToken);

        return new ResultInfo<ProductSearchAutocompleteResponseDto>(new ProductSearchAutocompleteResponseDto(Results: results));
    }
}