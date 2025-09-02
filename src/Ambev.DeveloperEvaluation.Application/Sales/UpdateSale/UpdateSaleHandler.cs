using Ambev.DeveloperEvaluation.Domain.Entities;
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
        var validator = new UpdateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existing = await _saleRepository.GetByIdAsync(command.Id, includeItems: true, cancellationToken);
        if (existing is null)
            throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

        _mapper.Map(command, existing);

        existing.Items.Clear();
        var newItems = _mapper.Map<List<SaleItem>>(command.Items);
        foreach (var it in newItems)
            existing.Items.Add(it);

        await _saleRepository.UpdateAsync(existing, cancellationToken);

        return _mapper.Map<UpdateSaleResult>(existing);
    }
}