namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.AddItem;

public class AddItemRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}