using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult?>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IQuantityDiscountService _quantityDiscountService;

    public UpdateSaleHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        IQuantityDiscountService quantityDiscountService)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _quantityDiscountService = quantityDiscountService;
    }

    public async Task<UpdateSaleResult?> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetByIdAsync(command.Id, includeItems: true, cancellationToken);
        if (sale is null)
            throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

        if (sale.IsCancelled)
            throw new DomainException("Cannot update a cancelled sale.");

        sale.SaleNumber = command.SaleNumber;
        sale.CreatedAt = command.CreatedAt;
        sale.CustomerId = command.CustomerId;
        sale.BranchId = command.BranchId;

        var newItems = _mapper.Map<List<SaleItem>>(command.Items);
        sale.UpdateItems(newItems, _quantityDiscountService);

        await _saleRepository.UpdateAsync(sale, cancellationToken);

        return _mapper.Map<UpdateSaleResult>(sale);
    }
}