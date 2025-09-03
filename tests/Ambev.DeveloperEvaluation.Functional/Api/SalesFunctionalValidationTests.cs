
using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Functional.Api.Factories;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.Api;

public class SalesFunctionalValidationTests : IClassFixture<ApiFactory>
{
    private readonly ApiFactory _factory;
    public SalesFunctionalValidationTests(ApiFactory factory) => _factory = factory;

    // [Fact(DisplayName = "Updating an item to >20 units should return 400")]
    // public async Task UpdateItem_OverLimit_ShouldReturnBadRequest()
    // {
    //     var client = _factory.CreateClientWithRole("Customer");
    //
    //     // cria venda com 1 item (válido)
    //     var create = await client.PostAsJsonAsync("/api/Sales", new {
    //         saleNumber = Guid.NewGuid(),
    //         createdAt = DateTime.UtcNow,
    //         customerId = Guid.NewGuid(),
    //         branchId = Guid.NewGuid(),
    //         items = new[] { new { productId = Guid.NewGuid(), quantity = 2, unitPrice = 10.0m } }
    //     });
    //     create.StatusCode.Should().Be(HttpStatusCode.Created);
    //
    //     var created = await create.Content.ReadFromJsonAsync<ApiResponse<CreateSaleResponse>>();
    //     var saleId = created!.Data.Id;
    //     var itemId = created.Data.Items[0].Id;
    //
    //     // tenta atualizar para 21 (inválido)
    //     var resp = await client.PutAsJsonAsync($"/api/Sales/{saleId}/items/{itemId}", new {
    //         quantity = 21,
    //         unitPrice = 10.0m
    //     });
    //
    //     resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    // }

    private class ApiResponse<T> { public bool Success { get; set; } public T Data { get; set; } = default!; }
    private class CreateSaleResponse { public Guid Id { get; set; } public List<CreateSaleItemResponse> Items { get; set; } = new(); }
    private class CreateSaleItemResponse { public Guid Id { get; set; } }
}
