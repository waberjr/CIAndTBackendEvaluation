namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateItem;

public class UpdateItemLineResult
{
    public Guid ProductId { get; set; }
    public int  Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}