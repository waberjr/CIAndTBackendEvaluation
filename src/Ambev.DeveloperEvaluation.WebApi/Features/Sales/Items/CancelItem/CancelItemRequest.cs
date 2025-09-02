using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.CancelItem;

public class CancelItemRequest
{
    [FromRoute(Name = "id")]
    public Guid SaleId { get; set; }

    [FromRoute(Name = "itemId")]
    public Guid ItemId { get; set; }
}