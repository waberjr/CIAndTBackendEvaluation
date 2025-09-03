using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.Items.AddItem;
using Ambev.DeveloperEvaluation.Application.Sales.Items.CancelItem;
using Ambev.DeveloperEvaluation.Application.Sales.Items.UpdateItem;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Common.Pagination;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.AddItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.CancelItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.UpdateItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

public class SalesController : BaseController
{
    #region Private Fields

    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    #endregion

    #region Constructors

    public SalesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    #endregion

    #region Sale

    [HttpPost]
    [Authorize(Roles = $"{nameof(UserRole.Customer)},{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateSaleCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<CreateSaleResponse>
        {
            Success = true,
            Message = "Sale created successfully",
            Data = _mapper.Map<CreateSaleResponse>(result)
        });
    }

    [HttpGet]
    [Authorize(Roles = $"{nameof(UserRole.Customer)},{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
    [ProducesResponseType(typeof(ApiResponseWithData<ListSalesResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ListSales([FromRoute] ListSalesRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<ListSalesCommand>(request);
        var result = await _mediator.Send(query, cancellationToken);

        var saleItems = _mapper.Map<List<ListSalesResponse>>(result.ToList());

        var paginatedResponse = new PaginatedList<ListSalesResponse>(saleItems, result.TotalCount, result.CurrentPage,
            result.PageSize);

        return OkPaginated(paginatedResponse);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = $"{nameof(UserRole.Customer)},{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSale([FromRoute] GetSaleRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetSaleCommand>(request.Id);
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(_mapper.Map<GetSaleResponse>(result), "Sale retrieved successfully");
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = $"{nameof(UserRole.Customer)},{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSale([FromRoute] Guid id, [FromBody] UpdateSaleRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateSaleCommand>(request);
        command.Id = id;
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(_mapper.Map<UpdateSaleResponse>(result), "Sale updated successfully");
    }

    [HttpPost("{id:guid}/cancel")]
    [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
    [ProducesResponseType(typeof(ApiResponseWithData<CancelSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelSale([FromRoute] CancelSaleRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<CancelSaleCommand>(request.Id);
        var result = await _mediator.Send(query, cancellationToken);

        var message = result.AlreadyCancelled
            ? "Sale was already cancelled"
            : "Sale cancelled successfully";

        return Ok(_mapper.Map<CancelSaleResponse>(result), message);
    }

    #endregion

    #region Sale Item

    [HttpPost("{id:guid}/items")]
    [Authorize(Roles = $"{nameof(UserRole.Customer)},{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
    [ProducesResponseType(typeof(ApiResponseWithData<AddItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddItem([FromRoute] Guid id, [FromBody] AddItemRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<AddItemCommand>(request);
        command.SaleId = id;
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(_mapper.Map<AddItemResponse>(result), "Item added successfully");
    }

    [HttpPut("{id:guid}/items/{itemId:guid}")]
    [Authorize(Roles = $"{nameof(UserRole.Customer)},{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateItem([FromRoute] Guid id, [FromRoute] Guid itemId,
        [FromBody] UpdateItemRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateItemCommand>(request);
        command.SaleId = id;
        command.ItemId = itemId;
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(_mapper.Map<UpdateItemResponse>(result), "Item updated successfully");
    }

    [HttpPost("{id:guid}/items/{itemId:guid}/cancel")]
    [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
    [ProducesResponseType(typeof(ApiResponseWithData<CancelItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelItem([FromRoute] CancelItemRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CancelItemCommand>(request);
        var result = await _mediator.Send(command, ct);

        return Ok(_mapper.Map<CancelItemResponse>(result), "Item cancelled successfully");
    }

    #endregion
}