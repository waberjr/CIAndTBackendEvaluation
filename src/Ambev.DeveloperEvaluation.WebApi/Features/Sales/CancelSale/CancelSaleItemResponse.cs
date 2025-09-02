namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

public class CancelSaleItemResponse
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}