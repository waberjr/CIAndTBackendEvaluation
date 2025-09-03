namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

public class UpdateSaleRequest
{
    public Guid SaleNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public List<UpdateSaleItemRequest> Items { get; set; } = [];
}