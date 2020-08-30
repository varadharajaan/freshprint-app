using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Supermarket.API.Domain.Models;
using Supermarket.API.Domain.Repositories;
using Supermarket.API.Persistence.Contexts;

namespace Supermarket.API.Persistence.Repositories
{
    public class ItemRepository : BaseRepository, IItemRepository
    {
        public ItemRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Item>> ListAsync()
        {
            return await _context.Items
                                 .AsNoTracking()
                                 .ToListAsync();

            // AsNoTracking tells EF Core it doesn't need to track changes on listed entities. Disabling entity
            // tracking makes the code a little faster
        }

        public async Task AddAsync(Item item)
        {
            await _context.Items.AddAsync(item);
        }

        public async Task<Item> FindByIdAsync(int id)
        {
            return await _context.Items.FindAsync(id);
        }

        public void Update(Item item)
        {
            _context.Items.Update(item);
        }

        public void Remove(Item item)
        {
            _context.Items.Remove(item);
        }
    }
}