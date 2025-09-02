using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.EventHandlers;

public class SaleCreatedEventHandler(ILogger<SaleCreatedEventHandler> logger) : INotificationHandler<SaleCreatedEvent>
{
    public Task Handle(SaleCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("SaleCreatedEvent handled for SaleId: {SaleId}", notification.Sale.Id);
        // other logic can be added here if needed

        return Task.CompletedTask;
    }
}