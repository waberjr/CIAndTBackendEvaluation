namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.UpdateItem;

public class UpdateItemResponse
{
    public Guid Id { get; set; }
    public Guid SaleNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public List<UpdateItemLineResponse> Items { get; set; } = [];
    public decimal TotalAmount { get; set; }
    public bool IsCancelled { get; set; }
}