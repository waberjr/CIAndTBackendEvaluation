namespace Ambev.DeveloperEvaluation.Application.Sales.Items.CancelItem;

public class CancelItemResult
{
    public Guid Id { get; set; }
    public Guid SaleNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public List<CancelItemLineResult> Items { get; set; } = [];
    public decimal TotalAmount { get; set; }
    public bool IsCancelled { get; set; }
}