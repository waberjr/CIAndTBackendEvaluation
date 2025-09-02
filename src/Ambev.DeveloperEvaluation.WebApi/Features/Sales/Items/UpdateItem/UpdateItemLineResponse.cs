namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.UpdateItem;

public class UpdateItemLineResponse
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}