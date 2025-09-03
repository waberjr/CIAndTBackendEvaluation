using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IQuantityDiscountService _quantityDiscountService;

    public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper,
        IQuantityDiscountService quantityDiscountService)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _quantityDiscountService = quantityDiscountService;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        // var sale = _mapper.Map<Sale>(command);
        var sale = new Sale
        {
            SaleNumber = command.SaleNumber,
            CreatedAt = command.CreatedAt,
            CustomerId = command.CustomerId,
            BranchId = command.BranchId
        };

        foreach (var item in command.Items)
        {
            sale.AddOrIncrementItem(
                productId: item.ProductId,
                quantity: item.Quantity,
                unitPrice: item.UnitPrice,
                policy: _quantityDiscountService
            );
        }

        sale.AddDomainEvent(new SaleCreatedEvent(sale));

        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);
        var result = _mapper.Map<CreateSaleResult>(createdSale);
        return result;
    }
}