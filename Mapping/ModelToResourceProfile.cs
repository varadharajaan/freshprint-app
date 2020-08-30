using AutoMapper;
using Product.API.Domain.Models;
using Product.API.Domain.Models.Queries;
using Product.API.Resources;


namespace Product.API.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Item, ItemResource>();
            
            CreateMap<QueryResult<Domain.Models.Product>, QueryResultResource<ProductResource>>();
        }
    }
}