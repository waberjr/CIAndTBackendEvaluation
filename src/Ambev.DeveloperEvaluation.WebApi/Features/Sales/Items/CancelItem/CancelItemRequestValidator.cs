using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.CancelItem;

public class CancelItemRequestValidator : AbstractValidator<CancelItemRequest>
{
    public CancelItemRequestValidator()
    {
        RuleFor(x => x.SaleId)
            .NotEmpty()
            .WithMessage("Sale ID is required");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");
    }
}