using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem : BaseAuditableEntity
{
    public Sale Sale { get; init; } = null!;
    public Guid ProductId { get; set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Discount { get; private set; }
    public decimal TotalPrice { get; private set; }
    public bool IsCancelled { get; private set; }

    public SaleItem(Sale sale, Guid productId, int quantity, decimal unitPrice,
        IQuantityDiscountService policy)
    {
        Sale = sale ?? throw new ArgumentNullException(nameof(sale));
        ProductId = productId;
        SetUnitPrice(unitPrice, policy);
        SetQuantity(quantity, policy);
    }

    internal SaleItem()
    {
    }

    public void SetQuantity(int quantity, IQuantityDiscountService policy)
    {
        if (IsCancelled) throw new DomainException("Cannot change a cancelled item.");
        if (quantity < 1) throw new DomainException("Quantity must be at least 1.");
        if (quantity > 20) throw new DomainException("Maximum 20 identical items per product.");

        Quantity = quantity;
        Recalculate(policy);
    }

    public void SetUnitPrice(decimal unitPrice, IQuantityDiscountService policy)
    {
        if (IsCancelled) throw new DomainException("Cannot change a cancelled item.");
        if (unitPrice <= 0) throw new DomainException("Unit price must be > 0.");

        UnitPrice = unitPrice;
        Recalculate(policy);
    }

    public void Cancel()
    {
        if (IsCancelled) return;
        IsCancelled = true;
    }

    public void Recalculate(IQuantityDiscountService policy)
    {
        Discount = policy.GetPercent(Quantity);
        var gross = Quantity * UnitPrice;
        var factor = 1m - (Discount / 100m);
        TotalPrice = Math.Round(gross * factor, 2, MidpointRounding.AwayFromZero);
    }
}