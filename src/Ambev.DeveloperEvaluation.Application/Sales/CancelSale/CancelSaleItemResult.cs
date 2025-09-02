namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public class CancelSaleItemResult
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}