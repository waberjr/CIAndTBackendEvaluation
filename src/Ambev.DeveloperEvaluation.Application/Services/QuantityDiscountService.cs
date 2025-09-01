using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Application.Services;

public class QuantityDiscountService : IQuantityDiscountService
{
    public decimal GetPercent(int quantity)
    {
        return quantity switch
        {
            < 4 => 0,
            < 10 => 10,
            _ => 20
        };
    }
}