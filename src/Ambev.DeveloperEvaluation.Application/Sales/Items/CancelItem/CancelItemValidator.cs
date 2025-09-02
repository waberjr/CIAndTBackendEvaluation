using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Items.CancelItem;

public class CancelItemValidator : AbstractValidator<CancelItemCommand>
{
    public CancelItemValidator()
    {
        RuleFor(x => x.SaleId).NotEmpty();
        RuleFor(x => x.ItemId).NotEmpty();
    }
}