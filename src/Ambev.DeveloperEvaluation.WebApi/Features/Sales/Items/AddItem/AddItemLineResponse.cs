namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.AddItem;

public class AddItemLineResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public bool IsCancelled { get; set; }
}