using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseAuditableEntity
{
    public Guid SaleNumber { get; set; }
    public Guid CustomerId { get; set; }
    public decimal TotalAmount { get; private set; }
    public Guid BranchId { get; set; }
    public bool IsCancelled { get; private set; }

    // todo: change to IReadOnlyCollection
    private readonly List<SaleItem> _items = [];
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

    public void UpdateItems(List<SaleItem> newItems, IQuantityDiscountService policy)
    {
        EnsureNotCancelled();
        _items.Clear();

        foreach (var saleItem in newItems)
        {
            AddOrIncrementItem(saleItem.ProductId, saleItem.Quantity, saleItem.UnitPrice, policy);
        }

        RecalculateTotal();
    }

    public SaleItem AddOrIncrementItem(Guid productId, int quantity, decimal unitPrice,
        IQuantityDiscountService policy)
    {
        EnsureNotCancelled();

        var existing = Items.FirstOrDefault(i => i.ProductId == productId && !i.IsCancelled);
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
        _items.Add(item);
        RecalculateTotal();

        AddDomainEvent(new SaleModifiedEvent(this));

        return item;
    }

    public void UpdateItemQuantity(Guid itemId, int newQuantity, IQuantityDiscountService policy)
    {
        EnsureNotCancelled();
        var item = Items.FirstOrDefault(i => i.Id == itemId)
                   ?? throw new DomainException("Item not found.");
        item.SetQuantity(newQuantity, policy);
        RecalculateTotal();
    }

    public void CancelItem(Guid itemId)
    {
        EnsureNotCancelled();
        var item = Items.FirstOrDefault(i => i.Id == itemId)
                   ?? throw new DomainException("Item not found.");
        item.Cancel();
        RecalculateTotal();
    }

    public void Cancel()
    {
        if (IsCancelled) return;
        IsCancelled = true;

        AddDomainEvent(new SaleCancelledEvent(this));
    }

    public void RecalculateTotal()
        => TotalAmount = Items.Where(i => !i.IsCancelled).Sum(i => i.TotalPrice);

    private void EnsureNotCancelled()
    {
        if (IsCancelled) throw new DomainException("Sale is cancelled.");
    }
}