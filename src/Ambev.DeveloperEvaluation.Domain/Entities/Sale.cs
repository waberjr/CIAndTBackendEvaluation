using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseAuditableEntity
{
    public Guid SaleNumber { get; init; }
    public Guid CustomerId { get; set; }
    public decimal TotalAmount { get; private set; }
    public Guid BranchId { get; set; }
    public bool IsCanceled { get; private set; }
    public List<SaleItem> Items { get; } = [];

    public SaleItem AddOrIncrementItem(Guid productId, int quantity, decimal unitPrice,
        IQuantityDiscountService policy)
    {
        EnsureNotCanceled();

        var existing = Items.FirstOrDefault(i => i.ProductId == productId && !i.IsCanceled);
        if (existing != null)
        {
            var newQty = existing.Quantity + quantity;
            if (newQty > 20)
                throw new DomainException("Maximum 20 identical items per product.");

            existing.SetUnitPrice(unitPrice, policy);
            existing.SetQuantity(newQty, policy);
            RecalculateTotal();
            return existing;
        }

        var item = new SaleItem(this, productId, quantity, unitPrice, policy);
        Items.Add(item);
        RecalculateTotal();
        return item;
    }

    public void UpdateItemQuantity(Guid itemId, int newQuantity, IQuantityDiscountService policy)
    {
        EnsureNotCanceled();
        var item = Items.FirstOrDefault(i => i.Id == itemId)
                   ?? throw new DomainException("Item not found.");
        item.SetQuantity(newQuantity, policy);
        RecalculateTotal();
    }

    public void CancelItem(Guid itemId)
    {
        EnsureNotCanceled();
        var item = Items.FirstOrDefault(i => i.Id == itemId)
                   ?? throw new DomainException("Item not found.");
        item.Cancel();
        RecalculateTotal();
    }

    public void Cancel()
    {
        if (IsCanceled) return;
        IsCanceled = true;
    }

    public void RecalculateTotal()
        => TotalAmount = Items.Where(i => !i.IsCanceled).Sum(i => i.TotalPrice);

    private void EnsureNotCanceled()
    {
        if (IsCanceled) throw new DomainException("Sale is canceled.");
    }
}