namespace Ambev.DeveloperEvaluation.Domain.Services;

public interface IQuantityDiscountService
{
    decimal GetPercent(int quantity);
}