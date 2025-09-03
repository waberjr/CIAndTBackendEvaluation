using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Items.CancelItem;

public class CancelItemHandler : IRequestHandler<CancelItemCommand, CancelItemResult?>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IQuantityDiscountService _quantityDiscountService;

    public CancelItemHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        IQuantityDiscountService quantityDiscountService)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _quantityDiscountService = quantityDiscountService;
    }

    public async Task<CancelItemResult?> Handle(CancelItemCommand cmd, CancellationToken ct)
    {
        var sale = await _saleRepository.GetByIdAsync(cmd.SaleId, includeItems: true, cancellationToken: ct);
        if (sale is null)
            throw new KeyNotFoundException($"Sale {cmd.SaleId} not found");

        var item = sale.Items.FirstOrDefault(i => i.Id == cmd.ItemId);
        if (item is null)
            throw new KeyNotFoundException($"Item with ID {cmd.ItemId} not found in Sale {cmd.SaleId}");

        if (!item.IsCancelled)
        {
            sale.CancelItem(item.Id);
        }


        await _saleRepository.UpdateAsync(sale, ct);

        return _mapper.Map<CancelItemResult>(sale);
    }
}