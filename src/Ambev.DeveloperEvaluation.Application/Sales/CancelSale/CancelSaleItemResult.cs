namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public class CancelSaleItemResult
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalPrice { get; set; }
    public bool IsCancelled { get; set; }
}