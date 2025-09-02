using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ambev.DeveloperEvaluation.WebApi.Filters;

public class FluentValidationActionFilter(IServiceProvider sp) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext ctx, ActionExecutionDelegate next)
    {
        foreach (var arg in ctx.ActionArguments.Values.Where(v => v is not null))
        {
            var argType = arg!.GetType();
            var validatorType = typeof(IValidator<>).MakeGenericType(argType);
            if (sp.GetService(validatorType) is not IValidator validator) continue;

            var context = new ValidationContext<object>(arg);
            var result = await validator.ValidateAsync(context, ctx.HttpContext.RequestAborted);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        await next();
    }
}