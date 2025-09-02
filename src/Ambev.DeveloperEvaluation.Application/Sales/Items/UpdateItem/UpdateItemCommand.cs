using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Items.UpdateItem;

public class UpdateItemCommand : IRequest<UpdateItemResult?>
{
    public Guid SaleId { get; set; }
    public Guid ItemId { get; set; }
    public int  Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}