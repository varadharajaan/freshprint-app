using System.Threading.Tasks;
using Product.API.Domain.Models.Queries;
using Product.API.Domain.Services.Communication;


namespace Product.API.Domain.Services
{
    public interface IProductService
    {
        Task<QueryResult<Models.Product>> ListAsync(ProductsQuery query);
        Task<ProductResponse> SaveAsync(Models.Product product);
        Task<ProductResponse> UpdateAsync(int id, Models.Product product);
        Task<ProductResponse> DeleteAsync(int id);
    }
}