using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.EventHandlers;

public class SaleModifiedEventHandler(ILogger<SaleModifiedEventHandler> logger) : INotificationHandler<SaleModifiedEvent>
{
    public Task Handle(SaleModifiedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("SaleModifiedEvent handled for SaleId: {SaleId}", notification.Sale.Id);
        // other logic can be added here if needed

        return Task.CompletedTask;
    }
}