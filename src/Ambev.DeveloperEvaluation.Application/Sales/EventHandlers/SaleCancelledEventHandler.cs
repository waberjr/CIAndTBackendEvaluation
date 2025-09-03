using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.EventHandlers;

public class SaleCancelledEventHandler(ILogger<SaleCancelledEventHandler> logger) : INotificationHandler<SaleCancelledEvent>
{
    public Task Handle(SaleCancelledEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("SaleCancelledEvent handled for SaleId: {SaleId}", notification.Sale.Id);
        // other logic can be added here if needed

        return Task.CompletedTask;
    }
}