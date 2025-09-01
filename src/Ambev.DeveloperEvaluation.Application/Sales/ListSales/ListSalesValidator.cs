using Ambev.DeveloperEvaluation.Common.Pagination;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public class ListSalesValidator : AbstractValidator<ListSalesCommand>
{
    public ListSalesValidator()
    {
        Include(new PaginatedRequestValidator());
    }
}