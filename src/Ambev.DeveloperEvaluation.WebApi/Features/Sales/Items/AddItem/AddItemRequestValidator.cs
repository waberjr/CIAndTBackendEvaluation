using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.AddItem;

public class AddItemRequestValidator : AbstractValidator<AddItemRequest>
{
    public AddItemRequestValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.Quantity).InclusiveBetween(1, 20);
        RuleFor(x => x.UnitPrice).GreaterThan(0);
    }
}