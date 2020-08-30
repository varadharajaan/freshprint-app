using System.Collections.Generic;
using System.Threading.Tasks;
using Supermarket.API.Domain.Models;
using Supermarket.API.Domain.Services.Communication;

namespace Supermarket.API.Domain.Services
{
    public interface IItemService
    {
         Task<IEnumerable<Item>> ListAsync();
         Task<ItemResponse> SaveAsync(Item item);
         Task<ItemResponse> UpdateAsync(int id, Item item);
         Task<ItemResponse> DeleteAsync(int id);
    }
}