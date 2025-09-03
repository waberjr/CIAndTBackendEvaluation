using FluentValidation;

namespace Ambev.DeveloperEvaluation.Common.Pagination;

public class PaginatedRequestValidator : AbstractValidator<PaginatedRequest>
{
    public PaginatedRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(AppConfiguration.MaxPageSize);
    }
}