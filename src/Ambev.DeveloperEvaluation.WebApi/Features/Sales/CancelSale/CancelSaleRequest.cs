using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

public class CancelSaleRequest
{
    [FromRoute(Name = "id")]
    public Guid Id { get; set; }
}