using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Services;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleTests
{
    private readonly IQuantityDiscountService _discounts = Substitute.For<IQuantityDiscountService>();

    [Fact]
    public void AddOrIncrementItem_SameProduct_ShouldMergeQuantitiesAndRecalculate()
    {
        // Arrange
        var sale = new Sale();
        var productId = Guid.NewGuid();

        _discounts.GetPercent(2).Returns(0);
        sale.AddOrIncrementItem(productId, 2, 10m, _discounts);

        _discounts.GetPercent(4).Returns(10); // after merge: 4 -> 10% off

        // Act
        sale.AddOrIncrementItem(productId, 2, 10m, _discounts);

        // Assert
        sale.Items.Should().HaveCount(1);
        var line = sale.Items.Single();
        line.Quantity.Should().Be(4);
        line.Discount.Should().Be(10);
        sale.TotalAmount.Should().Be(line.TotalPrice);
    }

    [Fact]
    public void CancelItem_ShouldRemoveItFromTotal()
    {
        var sale = new Sale();
        var p1 = Guid.NewGuid();
        var p2 = Guid.NewGuid();

        _discounts.GetPercent(4).Returns(10);
        var i1 = sale.AddOrIncrementItem(p1, 4, 10m, _discounts); // discounted
        _discounts.GetPercent(1).Returns(0);
        var i2 = sale.AddOrIncrementItem(p2, 1, 20m, _discounts); // no discount

        var totalBefore = sale.TotalAmount;

        // Act
        sale.CancelItem(i2.Id);

        // Assert
        sale.TotalAmount.Should().Be(totalBefore - i2.TotalPrice);
        sale.Items.Single(x => x.Id == i2.Id).IsCancelled.Should().BeTrue();
    }

    [Fact]
    public void CancelSale_ShouldBlockMutations()
    {
        var sale = new Sale();
        sale.Cancel();

        Action add = () => sale.AddOrIncrementItem(Guid.NewGuid(), 1, 1m, _discounts);
        add.Should().Throw<DomainException>().WithMessage("*Sale is cancelled*");
    }
}