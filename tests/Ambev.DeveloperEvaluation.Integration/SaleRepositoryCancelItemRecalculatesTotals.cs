using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Integration.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration;


public class SaleRepositoryCancelItemRecalculatesTotals : IClassFixture<SqliteDbFixture>
{
    private readonly SqliteDbFixture _fx;
    private readonly IQuantityDiscountService _discounts = Substitute.For<IQuantityDiscountService>();

    public SaleRepositoryCancelItemRecalculatesTotals(SqliteDbFixture fx) => _fx = fx;

    [Fact(DisplayName = "Cancel item should mark item and recalc Sale.TotalAmount (ignoring cancelled items)")]
    public async Task CancelItem_RecalculateTotals()
    {
        // Arrange: cria venda com 2 itens
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid()
        };

        _discounts.GetPercent(4).Returns(10); // item A
        var itemA = sale.AddOrIncrementItem(Guid.NewGuid(), 4, 10m, _discounts);

        _discounts.GetPercent(1).Returns(0); // item B
        var itemB = sale.AddOrIncrementItem(Guid.NewGuid(), 1, 20m, _discounts);

        var totalBefore = sale.TotalAmount;

        // Persist
        using (var ctx = _fx.NewContext())
        {
            ctx.Sales.Add(sale);
            await ctx.SaveChangesAsync();
        }

        // Act: cancela item B e salva
        using (var ctx = _fx.NewContext())
        {
            var loaded = await ctx.Sales.Include(s => s.Items)
                .FirstAsync(s => s.Id == sale.Id);

            var toCancel = loaded.Items.Single(i => i.Id == itemB.Id);
            toCancel.Cancel();
            loaded.RecalculateTotal();

            await ctx.SaveChangesAsync();
        }

        // Assert: reabre e confere
        using (var ctx = _fx.NewContext())
        {
            var after = await ctx.Sales.Include(s => s.Items)
                .FirstAsync(s => s.Id == sale.Id);

            after.Items.Single(i => i.Id == itemB.Id).IsCancelled.Should().BeTrue();
            after.TotalAmount.Should().Be(totalBefore - itemB.TotalPrice);
        }
    }
}
