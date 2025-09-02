using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.UpdateItem;

public class UpdateItemRequestValidator : AbstractValidator<UpdateItemRequest>
{
    public UpdateItemRequestValidator()
    {
        RuleFor(x => x.Quantity).InclusiveBetween(1, 20);
        RuleFor(x => x.UnitPrice).GreaterThan(0);
    }
}