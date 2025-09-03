using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Integration.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Infrastructure.Repositories;


public class SaleRepositoryIntegrationTests : IClassFixture<SqliteDbFixture>
{
    private readonly SqliteDbFixture _fx;
    private readonly IQuantityDiscountService _discounts = Substitute.For<IQuantityDiscountService>();

    public SaleRepositoryIntegrationTests(SqliteDbFixture fx) => _fx = fx;

    [Fact]
    public async Task Should_Save_And_Load_Sale_With_Items_And_Totals()
    {
        // Arrange
        var sale = new Sale
        {
            SaleNumber = Guid.NewGuid(),
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid()
        };

        _discounts.GetPercent(4).Returns(10);
        sale.AddOrIncrementItem(Guid.NewGuid(), 4, 10m, _discounts); // 10% off

        _discounts.GetPercent(2).Returns(0);
        sale.AddOrIncrementItem(Guid.NewGuid(), 2, 5m, _discounts); // no discount

        // Act
        using (var ctx = _fx.NewContext())
        {
            ctx.Sales.Add(sale);
            await ctx.SaveChangesAsync();
        }

        // Assert (load in new context)
        using (var ctx = _fx.NewContext())
        {
            var loaded = await ctx.Sales.Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == sale.Id);

            loaded.Should().NotBeNull();
            loaded!.Items.Should().HaveCount(2);
            loaded.TotalAmount.Should().Be(loaded.Items.Where(i => !i.IsCancelled).Sum(i => i.TotalPrice));
        }
    }
}
