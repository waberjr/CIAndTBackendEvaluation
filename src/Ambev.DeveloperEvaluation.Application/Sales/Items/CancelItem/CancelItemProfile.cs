using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.Items.CancelItem;

public class CancelItemProfile : Profile
{
    public CancelItemProfile()
    {
        CreateMap<Sale, CancelItemResult>();
        CreateMap<SaleItem, CancelItemLineResult>();
    }
}