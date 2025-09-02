using Ambev.DeveloperEvaluation.Application.Sales.Items.AddItem;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.AddItem;

public class AddItemProfile : Profile
{
    public AddItemProfile()
    {
        CreateMap<AddItemRequest, AddItemCommand>();

        CreateMap<AddItemResult, AddItemResponse>();
        CreateMap<AddItemLineResult, AddItemLineResponse>();
    }
}