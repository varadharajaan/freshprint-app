using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Product.API.Domain.Models;
using Product.API.Domain.Repositories;
using Product.API.Domain.Services;
using Product.API.Domain.Services.Communication;
using Product.API.Infrastructure;


namespace Product.API.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;

        public ItemService(IItemRepository itemRepository, IUnitOfWork unitOfWork, IMemoryCache cache)
        {
            _itemRepository = itemRepository;
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<IEnumerable<Item>> ListAsync()
        {
            // Here I try to get the items list from the memory cache. If there is no data in cache, the anonymous method will be
            // called, setting the cache to expire one minute ahead and returning the Task that lists the items from the repository.
            var items = await _cache.GetOrCreateAsync(CacheKeys.ItemsList, (entry) => {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                return _itemRepository.ListAsync();
            });
            
            return items;
        }

        public async Task<ItemResponse> SaveAsync(Item item)
        {
            try
            {
                await _itemRepository.AddAsync(item);
                await _unitOfWork.CompleteAsync();

                return new ItemResponse(item);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new ItemResponse($"An error occurred when saving the item: {ex.Message}");
            }
        }

        public async Task<ItemResponse> UpdateAsync(int id, Item item)
        {
            var existingItem = await _itemRepository.FindByIdAsync(id);

            if (existingItem == null)
                return new ItemResponse("Item not found.");

            existingItem.Name = item.Name;

            try
            {
                await _unitOfWork.CompleteAsync();

                return new ItemResponse(existingItem);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new ItemResponse($"An error occurred when updating the item: {ex.Message}");
            }
        }

        public async Task<ItemResponse> DeleteAsync(int id)
        {
            var existingItem = await _itemRepository.FindByIdAsync(id);

            if (existingItem == null)
                return new ItemResponse("Item not found.");

            try
            {
                _itemRepository.Remove(existingItem);
                await _unitOfWork.CompleteAsync();

                return new ItemResponse(existingItem);
            }
            catch (Exception ex)
            {
                // Do some logging stuff
                return new ItemResponse($"An error occurred when deleting the item: {ex.Message}");
            }
        }
    }
}
