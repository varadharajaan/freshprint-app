using AutoMapper;
using Product.API.Domain.Models;
using Product.API.Domain.Models.Queries;
using Product.API.Resources;


namespace Product.API.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<SaveItemResource, Item>();

            CreateMap<SaveProductResource, Domain.Models.Product>()
                .ForMember(src => src.UnitOfMeasurement, opt => opt.MapFrom(src => (EUnitOfMeasurement)src.UnitOfMeasurement));

            CreateMap<ProductsQueryResource, ProductsQuery>();
        }
    }
}