using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public class ListSalesHandler : IRequestHandler<ListSalesCommand, PaginatedList<ListSalesResult>>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public ListSalesHandler(
        ISaleRepository saleRepository,
        IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ListSalesResult>> Handle(ListSalesCommand request,
        CancellationToken cancellationToken)
    {
        var paginatedSales = await _saleRepository.GetAllAsync(request.PageNumber, request.PageSize,
            cancellationToken: cancellationToken);

        var saleItems = _mapper.Map<List<ListSalesResult>>(paginatedSales.ToList());

        return new PaginatedList<ListSalesResult>(
            saleItems, paginatedSales.TotalCount, paginatedSales.CurrentPage, paginatedSales.PageSize);
    }
}