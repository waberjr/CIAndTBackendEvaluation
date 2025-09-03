using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.EventHandlers;

public class SaleItemCancelledEventHandler(ILogger<SaleItemCancelledEventHandler> logger) : INotificationHandler<SaleItemCancelledEvent>
{
    public Task Handle(SaleItemCancelledEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("SaleItemCancelledEvent handled for SaleId: {SaleId}, ItemId: {ItemId}", notification.SaleItem.Sale.Id, notification.SaleItem.Id);
        // other logic can be added here if needed

        return Task.CompletedTask;
    }
}