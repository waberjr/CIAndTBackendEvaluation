
namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public class CancelSaleResult
{
    public Guid Id { get; set; }
    public Guid SaleNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public List<CancelSaleItemResult> Items { get; set; } = [];
    public decimal TotalAmount { get; set; }
    public bool IsCancelled { get; set; }

    public bool AlreadyCancelled { get; set; }
}