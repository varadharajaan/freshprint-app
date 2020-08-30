using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Supermarket.API.Domain.Models;
using Supermarket.API.Domain.Models.Queries;
using Supermarket.API.Domain.Services;
using Supermarket.API.Resources;


namespace Supermarket.API.Controllers.v2
{
    [Route("/api/v{version:apiVersion}/products")]
    [Produces("application/json")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        /// <summary>
        /// Lists all existing products.
        /// </summary>
        /// <returns>List of products.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(QueryResultResource<ProductResource>), 200)]
        public  ActionResult ListAsync([FromQuery] ProductsQueryResource query)
        {
            return Ok("2.0");
        }

        /// <summary>
        /// Saves a new product.
        /// </summary>
        /// <param name="resource">Product data.</param>
        /// <returns>Response for the request.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProductResource), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public ActionResult PostAsync([FromBody] SaveProductResource resource)
        {
            return Ok("2.0");
        }

        /// <summary>
        /// Updates an existing product according to an identifier.
        /// </summary>
        /// <param name="id">Product identifier.</param>
        /// <param name="resource">Product data.</param>
        /// <returns>Response for the request.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProductResource), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public ActionResult PutAsync(int id, [FromBody] SaveProductResource resource)
        {
            return Ok("2.0");
        }

        /// <summary>
        /// Deletes a given product according to an identifier.
        /// </summary>
        /// <param name="id">Product identifier.</param>
        /// <returns>Response for the request.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ProductResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public ActionResult DeleteAsync(int id)
        {
            return Ok("2.0");
        }
    }
}