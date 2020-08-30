using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Product.API.Domain.Models.Queries;
using Product.API.Domain.Repositories;
using Product.API.Domain.Services;
using Product.API.Domain.Services.Communication;
using Product.API.Infrastructure;

namespace Product.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;

        public ProductService(IProductRepository productRepository, IItemRepository itemRepository, IUnitOfWork unitOfWork, IMemoryCache cache)
        {
            _productRepository = productRepository;
            _itemRepository = itemRepository;
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<QueryResult<Domain.Models.Product>> ListAsync(ProductsQuery query)
        {
            // Here I list the query result from cache if they exist, but now the data can vary according to the item ID, page and amount of
            // items per page. I have to compose a cache to avoid returning wrong data.
            string cacheKey = GetCacheKeyForProductsQuery(query);
            
            var products = await _cache.GetOrCreateAsync(cacheKey, (entry) => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return _productRepository.ListAsync(query);
            });

            return products;
        }

        public async Task<ProductResponse> SaveAsync(Domain.Models.Product product)
        {
            try
            {
                /*
                 Notice here we have to check if the item ID is valid before adding the product, to avoid errors.
                 You can create a method into the ItemService class to return the item and inject the service here if you prefer, but 
                 it doesn't matter given the API scope.
                */
                var existingItem = await _itemRepository.FindByIdAsync(product.ItemId);
                if (existingItem == null)
                    return new ProductResponse("Invalid item.");

                await _productRepository.AddAsync(product);
                await _unitOfWork.CompleteAsync();

                return new ProductResponse(product);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new ProductResponse($"An error occurred when saving the product: {ex.Message}");
            }
        }

        public async Task<ProductResponse> UpdateAsync(int id, Domain.Models.Product product)
        {
            var existingProduct = await _productRepository.FindByIdAsync(id);

            if (existingProduct == null)
                return new ProductResponse("Product not found.");

            var existingItem = await _itemRepository.FindByIdAsync(product.ItemId);
            if (existingItem == null)
                return new ProductResponse("Invalid item.");

            existingProduct.Name = product.Name;
            existingProduct.UnitOfMeasurement = product.UnitOfMeasurement;
            existingProduct.QuantityInPackage = product.QuantityInPackage;
            existingProduct.ItemId = product.ItemId;

            try
            {
                _productRepository.Update(existingProduct);
                await _unitOfWork.CompleteAsync();

                return new ProductResponse(existingProduct);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new ProductResponse($"An error occurred when updating the product: {ex.Message}");
            }
        }

        public async Task<ProductResponse> DeleteAsync(int id)
        {
            var existingProduct = await _productRepository.FindByIdAsync(id);

            if (existingProduct == null)
                return new ProductResponse("Product not found.");

            try
            {
                _productRepository.Remove(existingProduct);
                await _unitOfWork.CompleteAsync();

                return new ProductResponse(existingProduct);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new ProductResponse($"An error occurred when deleting the product: {ex.Message}");
            }
        }

        private string GetCacheKeyForProductsQuery(ProductsQuery query)
        {
            string key = CacheKeys.ProductsList.ToString();
            
            if (query.ItemId.HasValue && query.ItemId > 0)
            {
                key = string.Concat(key, "_", query.ItemId.Value);
            }

            key = string.Concat(key, "_", query.Page, "_", query.ItemsPerPage);
            return key;
        }
    }
}