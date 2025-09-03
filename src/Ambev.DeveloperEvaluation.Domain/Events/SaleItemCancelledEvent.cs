using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleItemCancelledEvent(SaleItem saleItem) : BaseEvent
{
    public SaleItem SaleItem { get; } = saleItem;
}