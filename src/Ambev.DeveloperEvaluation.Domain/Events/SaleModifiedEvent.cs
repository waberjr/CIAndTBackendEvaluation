using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleModifiedEvent(Sale sale) : BaseEvent
{
    public Sale Sale { get; } = sale;
}