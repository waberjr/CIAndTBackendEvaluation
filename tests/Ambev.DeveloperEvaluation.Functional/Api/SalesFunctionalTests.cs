
using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Functional.Api.Factories;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.Api;


public class SalesFunctionalTests : IClassFixture<ApiFactory>
{
    private readonly ApiFactory _factory;
    public SalesFunctionalTests(ApiFactory factory) => _factory = factory;

    // [Fact]
    // public async Task POST_Sales_Should_Create_With_Discounts_And_Totals()
    // {
    //     var client = _factory.CreateClientWithRole("Customer");
    //
    //     var req = new
    //     {
    //         saleNumber = Guid.NewGuid(),
    //         createdAt = DateTime.UtcNow,
    //         customerId = Guid.NewGuid(),
    //         branchId   = Guid.NewGuid(),
    //         items = new[]
    //         {
    //             new { productId = Guid.NewGuid(), quantity = 3,  unitPrice = 10.00m }, // 0%
    //             new { productId = Guid.NewGuid(), quantity = 10, unitPrice = 5.00m }   // 20%
    //         }
    //     };
    //
    //     var resp = await client.PostAsJsonAsync("/api/Sales", req);
    //     resp.StatusCode.Should().Be(HttpStatusCode.Created);
    //
    //     var body = await resp.Content.ReadFromJsonAsync<ApiResponse<CreateSaleResponse>>();
    //     body!.Success.Should().BeTrue();
    //     body.Data.Items.Should().HaveCount(2);
    //
    //     var i1 = body.Data.Items[0];
    //     var i2 = body.Data.Items[1];
    //
    //     i1.Discount.Should().Be(0);
    //     i2.Discount.Should().Be(20);
    //
    //     var expectedTotal = i1.TotalPrice + i2.TotalPrice;
    //     body.Data.TotalAmount.Should().Be(expectedTotal);
    // }

    // [Fact]
    // public async Task POST_Sales_Cancel_Should_Require_ManagerOrAdmin()
    // {
    //     var clientCustomer = _factory.CreateClientWithRole("Customer");
    //     var id = Guid.NewGuid(); // supondo venda existente/seed – em cenário real, crie primeiro e use o ID
    //
    //     var respForbidden = await clientCustomer.PostAsync($"/api/Sales/{id}/cancel", null);
    //     respForbidden.StatusCode.Should().Match(p => p == HttpStatusCode.Forbidden || p == HttpStatusCode.Unauthorized);
    //
    //     var clientManager = _factory.CreateClientWithRole("Manager");
    //     var respOk = await clientManager.PostAsync($"/api/Sales/{id}/cancel", null);
    //     respOk.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound); // OK se existir, 404 se não
    // }

    private class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; } = default!;
    }

    private class CreateSaleResponse
    {
        public Guid Id { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CreateSaleItemResponse> Items { get; set; } = new();
    }

    private class CreateSaleItemResponse
    {
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
