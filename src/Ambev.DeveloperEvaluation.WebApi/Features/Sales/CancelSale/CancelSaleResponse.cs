using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

public class CancelSaleResponse
{
    public Guid Id { get; set; }
    public Guid SaleNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public List<CancelSaleItemResponse> Items { get; set; } = [];
    public decimal TotalAmount { get; set; }
    public bool IsCancelled { get; set; }
}