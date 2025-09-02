using Ambev.DeveloperEvaluation.Application.Sales.AddItem;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.AddItem;

public class AddItemProfile : Profile
{
    public AddItemProfile()
    {
        CreateMap<AddItemRequest, AddItemCommand>();

        CreateMap<AddItemResult, AddItemResponse>();
        CreateMap<AddItemLineResult, AddItemLineResponse>();
    }
}