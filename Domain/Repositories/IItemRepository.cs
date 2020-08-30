using System.Collections.Generic;
using System.Threading.Tasks;
using Product.API.Domain.Models;

namespace Product.API.Domain.Repositories
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> ListAsync();
        Task AddAsync(Item item);
        Task<Item> FindByIdAsync(int id);
        void Update(Item item);
        void Remove(Item item);
    }
}