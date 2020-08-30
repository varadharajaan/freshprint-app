using System.Collections.Generic;
using System.Threading.Tasks;
using Product.API.Domain.Models;
using Product.API.Domain.Services.Communication;


namespace Product.API.Domain.Services
{
    public interface IItemService
    {
         Task<IEnumerable<Item>> ListAsync();
         Task<ItemResponse> SaveAsync(Item item);
         Task<ItemResponse> UpdateAsync(int id, Item item);
         Task<ItemResponse> DeleteAsync(int id);
    }
}