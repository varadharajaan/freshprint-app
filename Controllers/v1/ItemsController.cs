using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Product.API.Domain.Models;
using Product.API.Domain.Services;
using Product.API.Resources;


namespace Product.API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/items")]
    [Produces("application/json")]
    [ApiController]
    public class ItemsController : Controller
    {
        private readonly IItemService _itemService;
        private readonly IMapper _mapper;

        public ItemsController(IItemService itemService, IMapper mapper)
        {
            _itemService = itemService;
            _mapper = mapper;
        }

        /// <summary>
        /// Lists all items.
        /// </summary>
        /// <returns>List os items.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ItemResource>), 200)]
        public async Task<IEnumerable<ItemResource>> ListAsync()
        {
            var items = await _itemService.ListAsync();
            var resources = _mapper.Map<IEnumerable<Item>, IEnumerable<ItemResource>>(items);

            return resources;
        }

        /// <summary>
        /// Saves a new item.
        /// </summary>
        /// <param name="resource">Item data.</param>
        /// <returns>Response for the request.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ItemResource), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PostAsync([FromBody] SaveItemResource resource)
        {
            var item = _mapper.Map<SaveItemResource, Item>(resource);
            var result = await _itemService.SaveAsync(item);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var itemResource = _mapper.Map<Item, ItemResource>(result.Resource);
            return Ok(itemResource);
        }

        /// <summary>
        /// Updates an existing item according to an identifier.
        /// </summary>
        /// <param name="id">Item identifier.</param>
        /// <param name="resource">Updated item data.</param>
        /// <returns>Response for the request.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ItemResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> PutAsync(int id, [FromBody] SaveItemResource resource)
        {
            var item = _mapper.Map<SaveItemResource, Item>(resource);
            var result = await _itemService.UpdateAsync(id, item);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var itemResource = _mapper.Map<Item, ItemResource>(result.Resource);
            return Ok(itemResource);
        }

        /// <summary>
        /// Deletes a given item according to an identifier.
        /// </summary>
        /// <param name="id">Item identifier.</param>
        /// <returns>Response for the request.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ItemResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _itemService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var itemResource = _mapper.Map<Item, ItemResource>(result.Resource);
            return Ok(itemResource);
        }
    }
}