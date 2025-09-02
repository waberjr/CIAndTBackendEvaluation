namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.CancelItem;

public class CancelItemRequest
{
    public Guid SaleId { get; set; }
    public Guid ProductId { get; set; }
}