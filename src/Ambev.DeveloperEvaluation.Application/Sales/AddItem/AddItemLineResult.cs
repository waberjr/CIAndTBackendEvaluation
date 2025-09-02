namespace Ambev.DeveloperEvaluation.Application.Sales.AddItem;

public class AddItemLineResult
{
    public Guid ProductId { get; set; }
    public int  Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}