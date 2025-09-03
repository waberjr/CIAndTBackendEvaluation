
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Integration.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration;


public class SaleRepositoryReplaceItemsRecalculatesTotals : IClassFixture<SqliteDbFixture>
{
    private readonly SqliteDbFixture _fx;
    private readonly IQuantityDiscountService _discounts = Substitute.For<IQuantityDiscountService>();

    public SaleRepositoryReplaceItemsRecalculatesTotals(SqliteDbFixture fx) => _fx = fx;

    [Fact(DisplayName = "Replacing items should recalc totals using discount policy")]
    public async Task ReplaceItems_RecalculateTotals()
    {
        // Arrange: cria venda com 1 item (4 unidades, 10%)
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid()
        };
        _discounts.GetPercent(4).Returns(10);
        sale.AddOrIncrementItem(Guid.NewGuid(), 4, 10m, _discounts);

        using (var ctx = _fx.NewContext())
        {
            ctx.Sales.Add(sale);
            await ctx.SaveChangesAsync();
        }

        // Act: substitui por dois itens (9->10% e 10->20%)
        var p1 = Guid.NewGuid(); var p2 = Guid.NewGuid();
        _discounts.GetPercent(9).Returns(10);
        _discounts.GetPercent(10).Returns(20);

        using (var ctx = _fx.NewContext())
        {
            var loaded = await ctx.Sales.Include(s => s.Items)
                .FirstAsync(s => s.Id == sale.Id);

            var newItems = new List<SaleItem>
            {
                // construtor exige Sale para manter invariantes -> use o próprio loaded (agregado raiz)
                new SaleItem(loaded, p1, 9, 10m, _discounts),
                new SaleItem(loaded, p2, 10, 5m, _discounts)
            };

            loaded.UpdateItems(newItems, _discounts);
            await ctx.SaveChangesAsync();
        }

        // Assert: carregar e checar totais
        using (var ctx = _fx.NewContext())
        {
            var after = await ctx.Sales.Include(s => s.Items)
                .FirstAsync(s => s.Id == sale.Id);

            after.Items.Should().HaveCount(2);
            after.TotalAmount.Should().Be(after.Items.Where(i => !i.IsCancelled).Sum(i => i.TotalPrice));
            after.Items.Any(i => i.Discount is 10 or 20).Should().BeTrue();
        }
    }
}
