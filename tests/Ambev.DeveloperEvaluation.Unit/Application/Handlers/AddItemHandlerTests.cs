using Ambev.DeveloperEvaluation.Application.Sales.Items.AddItem;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Handlers;

public class AddItemHandlerTests
{
    private readonly ISaleRepository _repo = Substitute.For<ISaleRepository>();
    private readonly IQuantityDiscountService _discounts = Substitute.For<IQuantityDiscountService>();

    [Fact]
    public async Task Should_AddItem_And_RecalculateTotals()
    {
        // Arrange
        var sale = new Sale();
        _repo.GetByIdAsync(sale.Id, includeItems: true, Arg.Any<CancellationToken>()).Returns(sale);
        _discounts.GetPercent(5).Returns(10);

        var handler = new AddItemHandler(_repo, /* mapper not needed here */ NSubstitute.Substitute.For<AutoMapper.IMapper>(), _discounts);
        var cmd = new AddItemCommand { SaleId = sale.Id, ProductId = Guid.NewGuid(), Quantity = 5, UnitPrice = 10m };

        // Act
        var result = await handler.Handle(cmd, CancellationToken.None);

        // Assert
        sale.Items.Should().ContainSingle();
        sale.TotalAmount.Should().Be(sale.Items.Sum(i => i.TotalPrice));
        await _repo.Received(1).UpdateAsync(sale, Arg.Any<CancellationToken>());
    }
}