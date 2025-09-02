using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateItem;

public class UpdateItemCommand : IRequest<UpdateItemResult?>
{
    public Guid SaleId { get; set; }
    public Guid ProductId { get; set; }
    public int  Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}