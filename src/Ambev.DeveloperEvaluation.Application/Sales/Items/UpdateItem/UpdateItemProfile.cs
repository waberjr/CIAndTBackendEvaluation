using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.Items.UpdateItem;

public class UpdateItemProfile : Profile
{
    public UpdateItemProfile()
    {
        CreateMap<Sale, UpdateItemResult>();
        CreateMap<SaleItem, UpdateItemLineResult>();
    }
}