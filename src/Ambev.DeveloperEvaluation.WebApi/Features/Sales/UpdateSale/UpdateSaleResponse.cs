namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

public class UpdateSaleResponse
{
    public Guid Id { get; set; }
    public Guid SaleNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public List<UpdateSaleItemResponse> Items { get; set; } = [];
    public decimal TotalAmount { get; set; }
    public bool IsCancelled { get; set; }
}