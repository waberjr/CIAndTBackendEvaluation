namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleResult
{
    public Guid Id { get; set; }
    public Guid SaleNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public List<UpdateSaleItemResult> Items { get; set; } = [];
    public decimal TotalAmount { get; set; }
    public bool IsCancelled { get; set; }
}