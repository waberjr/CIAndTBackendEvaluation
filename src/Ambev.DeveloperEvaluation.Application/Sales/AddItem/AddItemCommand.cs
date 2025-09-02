using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.AddItem;

public class AddItemCommand : IRequest<AddItemResult?>
{
    public Guid SaleId { get; set; }
    public Guid ProductId { get; set; }
    public int  Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}