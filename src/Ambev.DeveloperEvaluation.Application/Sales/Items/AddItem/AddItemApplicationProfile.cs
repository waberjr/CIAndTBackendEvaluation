using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.Items.AddItem;

public class AddItemApplicationProfile : Profile
{
    public AddItemApplicationProfile()
    {
        CreateMap<Sale, AddItemResult>();
        CreateMap<SaleItem, AddItemLineResult>();
    }
}