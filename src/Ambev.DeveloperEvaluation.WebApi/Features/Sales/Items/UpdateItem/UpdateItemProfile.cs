using Ambev.DeveloperEvaluation.Application.Sales.Items.UpdateItem;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Items.UpdateItem;

public class UpdateItemProfile : Profile
{
    public UpdateItemProfile()
    {
        CreateMap<UpdateItemRequest, UpdateItemCommand>();

        CreateMap<UpdateItemResult, UpdateItemResponse>();
        CreateMap<UpdateItemLineResult, UpdateItemLineResponse>();
    }
}