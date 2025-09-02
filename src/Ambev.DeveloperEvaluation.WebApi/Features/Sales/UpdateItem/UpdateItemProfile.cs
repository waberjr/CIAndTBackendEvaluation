using Ambev.DeveloperEvaluation.Application.Sales.UpdateItem;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateItem;

public class UpdateItemProfile : Profile
{
    public UpdateItemProfile()
    {
        CreateMap<UpdateItemRequest, UpdateItemCommand>();

        CreateMap<UpdateItemResult, UpdateItemResponse>();
    }
}