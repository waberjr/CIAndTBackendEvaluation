using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Items.UpdateItem;

public class UpdateItemHandler : IRequestHandler<UpdateItemCommand, UpdateItemResult?>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IQuantityDiscountService _quantityDiscountService;

    public UpdateItemHandler(ISaleRepository saleRepository, IMapper mapper, IQuantityDiscountService qds)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _quantityDiscountService = qds;
    }

    public async Task<UpdateItemResult?> Handle(UpdateItemCommand command, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(command.SaleId, includeItems: true, cancellationToken);
        if (sale is null)
            throw new KeyNotFoundException($"Sale with ID {command.SaleId} not found");

        if (sale.IsCancelled)
            throw new DomainException("Cannot update items in a cancelled sale.");

        var item = sale.Items.FirstOrDefault(i => i.Id == command.ItemId);
        if (item is null)
            throw new KeyNotFoundException($"Item with ID {command.ItemId} not found in Sale ID {command.SaleId}");

        item.SetQuantity(command.Quantity, _quantityDiscountService);
        item.SetUnitPrice(command.UnitPrice, _quantityDiscountService);

        await _saleRepository.UpdateAsync(sale, cancellationToken);

        return _mapper.Map<UpdateItemResult>(sale);
    }
}