using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Product.API.Domain.Models.Queries;
using Product.API.Domain.Repositories;
using Product.API.Persistence.Contexts;


namespace Product.API.Persistence.Repositories
{
	public class ProductRepository : BaseRepository, IProductRepository
	{
		public ProductRepository(AppDbContext context) : base(context) { }

		public async Task<QueryResult<Domain.Models.Product>> ListAsync(ProductsQuery query)
		{
			IQueryable<Domain.Models.Product> queryable = _context.Products
													.Include(p => p.Item)
													.AsNoTracking();

			// AsNoTracking tells EF Core it doesn't need to track changes on listed entities. Disabling entity
			// tracking makes the code a little faster
			if (query.ItemId.HasValue && query.ItemId > 0)
			{
				queryable = queryable.Where(p => p.ItemId == query.ItemId);
			}

			// Here I count all items present in the database for the given query, to return as part of the pagination data.
			int totalItems = await queryable.CountAsync();

			// Here I apply a simple calculation to skip a given number of items, according to the current page and amount of items per page,
			// and them I return only the amount of desired items. The methods "Skip" and "Take" do the trick here.
			List<Domain.Models.Product> products = await queryable.Skip((query.Page - 1) * query.ItemsPerPage)
													.Take(query.ItemsPerPage)
													.ToListAsync();

			// Finally I return a query result, containing all items and the amount of items in the database (necessary for client-side calculations ).
			return new QueryResult<Domain.Models.Product>
			{
				Items = products,
				TotalItems = totalItems,
			};
		}

		public async Task<Domain.Models.Product> FindByIdAsync(int id)
		{
			return await _context.Products
								 .Include(p => p.Item)
								 .FirstOrDefaultAsync(p => p.Id == id); // Since Include changes the method's return type, we can't use FindAsync
		}

		public async Task AddAsync(Domain.Models.Product product)
		{
			await _context.Products.AddAsync(product);
		}

		public void Update(Domain.Models.Product product)
		{
			_context.Products.Update(product);
		}

		public void Remove(Domain.Models.Product product)
		{
			_context.Products.Remove(product);
		}
	}
}