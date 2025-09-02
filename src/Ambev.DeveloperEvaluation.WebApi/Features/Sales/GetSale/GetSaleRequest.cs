using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

public class GetSaleRequest
{
    [FromRoute(Name = "id")]
    public Guid Id { get; set; }
}