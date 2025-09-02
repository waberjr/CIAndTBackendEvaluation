using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Items.AddItem;

public class AddItemCommandValidator : AbstractValidator<AddItemCommand>
{
    public AddItemCommandValidator()
    {
        RuleFor(x => x.SaleId).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Quantity).InclusiveBetween(1, 20);
        RuleFor(x => x.UnitPrice).GreaterThan(0);
    }
}