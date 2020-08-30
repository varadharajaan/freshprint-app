using System.Collections.Generic;
using System.Threading.Tasks;
using Product.API.Domain.Models.Queries;

namespace Product.API.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<QueryResult<Models.Product>> ListAsync(ProductsQuery query);
        Task AddAsync(Models.Product product);
        Task<Models.Product> FindByIdAsync(int id);
        void Update(Models.Product product);
        void Remove(Models.Product product);
    }
}