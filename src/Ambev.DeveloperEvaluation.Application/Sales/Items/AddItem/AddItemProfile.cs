using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.Items.AddItem;

public class AddItemProfile : Profile
{
    public AddItemProfile()
    {
        CreateMap<Sale, AddItemResult>();
        CreateMap<SaleItem, AddItemLineResult>();
    }
}