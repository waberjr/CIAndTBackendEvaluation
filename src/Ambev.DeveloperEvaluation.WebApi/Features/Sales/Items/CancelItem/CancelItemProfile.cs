using Ambev.DeveloperEvaluation.Application.Sales.Items.CancelItem;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.CancelItem;

public class CancelItemProfile : Profile
{
    public CancelItemProfile()
    {
        CreateMap<CancelItemRequest, CancelItemCommand>();

        CreateMap<CancelItemResult, CancelItemResponse>();
        CreateMap<CancelItemLineResult, CancelItemLineResponse>();
    }
}