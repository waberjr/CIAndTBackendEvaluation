using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.Common.Pagination;

public class PaginatedRequest
{
    [FromQuery(Name = "_page")]
    public int PageNumber { get; init; } = AppConfiguration.DefaultPageNumber;

    [FromQuery(Name = "_size")]
    public int PageSize { get; init; } = AppConfiguration.DefaultPageSize;
}