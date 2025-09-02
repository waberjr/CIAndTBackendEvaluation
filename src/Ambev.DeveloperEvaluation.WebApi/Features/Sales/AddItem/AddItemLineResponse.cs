namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.AddItem;

public class AddItemLineResponse
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}