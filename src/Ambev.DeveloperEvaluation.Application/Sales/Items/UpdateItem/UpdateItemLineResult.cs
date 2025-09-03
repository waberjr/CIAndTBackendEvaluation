namespace Ambev.DeveloperEvaluation.Application.Sales.Items.UpdateItem;

public class UpdateItemLineResult
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int  Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalPrice { get; set; }
    public bool IsCancelled { get; set; }
}