
using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Functional.Api.Factories;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.Api;

public class SalesFunctionalFullFlowTests : IClassFixture<ApiFactory>
{
    private readonly ApiFactory _factory;
    public SalesFunctionalFullFlowTests(ApiFactory factory) => _factory = factory;

    // [Fact(DisplayName = "End-to-end sale flow should compute discounts, totals and enforce roles")]
    // public async Task FullFlow_Sale_Crud_And_Cancellations()
    // {
    //     // 1) Create sale (Customer ok)
    //     var clientCustomer = _factory.CreateClientWithRole("Customer");
    //     var createReq = new
    //     {
    //         saleNumber = Guid.NewGuid(),
    //         createdAt = DateTime.UtcNow,
    //         customerId = Guid.NewGuid(),
    //         branchId = Guid.NewGuid(),
    //         items = new[]
    //         {
    //             new { productId = Guid.NewGuid(), quantity = 3,  unitPrice = 10.00m }, // 0%
    //             new { productId = Guid.NewGuid(), quantity = 10, unitPrice = 5.00m }   // 20%
    //         }
    //     };
    //
    //     var createResp = await clientCustomer.PostAsJsonAsync("/api/Sales", createReq);
    //     createResp.StatusCode.Should().Be(HttpStatusCode.Created);
    //
    //     var created = await createResp.Content.ReadFromJsonAsync<ApiResponse<CreateSaleResponse>>();
    //     created!.Success.Should().BeTrue();
    //
    //     var saleId = created.Data.Id;
    //     var itemIdToUpdate = created.Data.Items[0].Id; // o de 3 un
    //     var itemIdToCancel = created.Data.Items[1].Id; // o de 10 un
    //
    //     // 2) AddItem (Customer ok)
    //     var addReq = new { productId = Guid.NewGuid(), quantity = 4, unitPrice = 10.00m }; // 10%
    //     var addResp = await clientCustomer.PostAsJsonAsync($"/api/Sales/{saleId}/items", addReq);
    //     addResp.StatusCode.Should().Be(HttpStatusCode.OK);
    //
    //     var afterAdd = await addResp.Content.ReadFromJsonAsync<ApiResponse<AddItemResponse>>();
    //     afterAdd!.Data.Items.Should().HaveCount(3);
    //
    //     // 3) UpdateItem (-> 12 un => 20%)
    //     var updReq = new { quantity = 12, unitPrice = 9.50m };
    //     var updResp = await clientCustomer.PutAsJsonAsync($"/api/Sales/{saleId}/items/{itemIdToUpdate}", updReq);
    //     updResp.StatusCode.Should().Be(HttpStatusCode.OK);
    //
    //     var afterUpd = await updResp.Content.ReadFromJsonAsync<ApiResponse<UpdateItemResponse>>();
    //     var updatedLine = afterUpd!.Data.Items.Single(i => i.Id == itemIdToUpdate);
    //     updatedLine.Discount.Should().Be(20);
    //
    //     // 4) CancelItem (Customer NÃO pode; Manager pode)
    //     var forbidCancel = await clientCustomer.PostAsync($"/api/Sales/{saleId}/items/{itemIdToCancel}/cancel", null);
    //     forbidCancel.StatusCode.Should().BeOneOf(HttpStatusCode.Forbidden, HttpStatusCode.Unauthorized);
    //
    //     var clientManager = _factory.CreateClientWithRole("Manager");
    //     var okCancel = await clientManager.PostAsync($"/api/Sales/{saleId}/items/{itemIdToCancel}/cancel", null);
    //     okCancel.StatusCode.Should().Be(HttpStatusCode.OK);
    //
    //     var afterItemCancel = await okCancel.Content.ReadFromJsonAsync<ApiResponse<CancelItemResponse>>();
    //     afterItemCancel!.Data.Items.Single(i => i.Id == itemIdToCancel).IsCancelled.Should().BeTrue();
    //
    //     // 5) CancelSale (Customer NÃO pode; Manager/Admin pode)
    //     var forbidSaleCancel = await clientCustomer.PostAsync($"/api/Sales/{saleId}/cancel", null);
    //     forbidSaleCancel.StatusCode.Should().BeOneOf(HttpStatusCode.Forbidden, HttpStatusCode.Unauthorized);
    //
    //     var okSaleCancel = await clientManager.PostAsync($"/api/Sales/{saleId}/cancel", null);
    //     okSaleCancel.StatusCode.Should().Be(HttpStatusCode.OK);
    //
    //     var afterSaleCancel = await okSaleCancel.Content.ReadFromJsonAsync<ApiResponse<CancelSaleResponse>>();
    //     afterSaleCancel!.Data.IsCancelled.Should().BeTrue();
    // }

    // tipos mínimos para desserializar sua ApiResponse
    private class ApiResponse<T> { public bool Success { get; set; } public T Data { get; set; } = default!; }

    private class CreateSaleResponse
    {
        public Guid Id { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CreateSaleItemResponse> Items { get; set; } = new();
    }
    private class CreateSaleItemResponse
    {
        public Guid Id { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalPrice { get; set; }
    }

    private class AddItemResponse
    {
        public Guid Id { get; set; }
        public List<AddItemLineResponse> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }
    }
    private class AddItemLineResponse
    {
        public Guid Id { get; set; }
        public decimal Discount { get; set; }
        public bool IsCancelled { get; set; }
    }

    private class UpdateItemResponse
    {
        public Guid Id { get; set; }
        public List<UpdateItemLineResponse> Items { get; set; } = new();
    }
    private class UpdateItemLineResponse
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
    }

    private class CancelItemResponse
    {
        public Guid Id { get; set; }
        public List<CancelItemLineResponse> Items { get; set; } = new();
    }
    private class CancelItemLineResponse
    {
        public Guid Id { get; set; }
        public bool IsCancelled { get; set; }
    }

    private class CancelSaleResponse
    {
        public Guid Id { get; set; }
        public bool IsCancelled { get; set; }
    }
}
