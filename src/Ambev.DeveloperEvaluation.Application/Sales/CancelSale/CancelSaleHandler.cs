using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public CancelSaleHandler(
        ISaleRepository saleRepository,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<CancelSaleResult> Handle(CancelSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CancelSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken: cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

        var already = sale.IsCancelled;
        if (!already)
        {
            sale.Cancel();
            await _saleRepository.UpdateAsync(sale, cancellationToken: cancellationToken);
        }

        var result = _mapper.Map<CancelSaleResult>(sale);
        result.AlreadyCancelled = already;
        return result;
    }
}