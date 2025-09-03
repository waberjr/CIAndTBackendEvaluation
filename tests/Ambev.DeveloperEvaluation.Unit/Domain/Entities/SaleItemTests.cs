using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Services;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleItemTests
{
    private readonly IQuantityDiscountService _discounts = Substitute.For<IQuantityDiscountService>();

    [Theory]
    [InlineData(1, 0)]
    [InlineData(3, 0)]
    [InlineData(4, 10)]
    [InlineData(9, 10)]
    [InlineData(10, 20)]
    [InlineData(20, 20)]
    public void Recalculate_ShouldApplyExpectedDiscount(int qty, decimal expectedPct)
    {
        // Arrange
        var sale = new Sale();
        _discounts.GetPercent(qty).Returns(expectedPct);

        // Act
        var item = new SaleItem(sale, Guid.NewGuid(), qty, 10m, _discounts);

        // Assert
        item.Discount.Should().Be(expectedPct);
        var expectedTotal = Math.Round(qty * 10m * (1m - expectedPct/100m), 2, MidpointRounding.AwayFromZero);
        item.TotalPrice.Should().Be(expectedTotal);
    }

    [Fact]
    public void SetQuantity_MoreThan20_ShouldThrow()
    {
        var sale = new Sale();
        _discounts.GetPercent(21).Returns(20);

        var act = () => new SaleItem(sale, Guid.NewGuid(), 21, 10m, _discounts);
        act.Should().Throw<DomainException>()
            .WithMessage("*Maximum 20 identical items*");
    }

    [Fact]
    public void Cancel_ShouldBlockFurtherChanges()
    {
        var sale = new Sale();
        _discounts.GetPercent(5).Returns(10);
        var item = new SaleItem(sale, Guid.NewGuid(), 5, 10m, _discounts);

        item.Cancel();

        Action change = () => item.SetUnitPrice(9m, _discounts);
        change.Should().Throw<DomainException>()
            .WithMessage("*Cannot change a cancelled item*");
    }
}