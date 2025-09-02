using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Items.AddItem;

public class AddItemHandler : IRequestHandler<AddItemCommand, AddItemResult?>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IQuantityDiscountService _quantityDiscountService;

    public AddItemHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        IQuantityDiscountService quantityDiscountService)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _quantityDiscountService = quantityDiscountService;
    }

    public async Task<AddItemResult?> Handle(AddItemCommand command, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken: cancellationToken);
        if (sale is null)
            throw new KeyNotFoundException($"Sale with ID {command.SaleId} not found");

        if (sale.IsCancelled)
            throw new InvalidOperationException("Cannot add items to a cancelled sale.");

        var existing = sale.Items.FirstOrDefault(i =>
            i.ProductId == command.ProductId
        );

        if (existing is not null)
            throw new InvalidOperationException("Item with the same ProductId already exists in the sale.");

        sale.AddOrIncrementItem(command.ProductId, command.Quantity, command.UnitPrice, _quantityDiscountService);

        await _saleRepository.UpdateAsync(sale, cancellationToken);

        return _mapper.Map<AddItemResult>(sale);
    }
}