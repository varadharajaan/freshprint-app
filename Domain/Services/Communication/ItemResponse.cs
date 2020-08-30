using Product.API.Domain.Models;

namespace Product.API.Domain.Services.Communication
{
    public class ItemResponse : BaseResponse<Item>
    {
        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="item">Saved item.</param>
        /// <returns>Response.</returns>
        public ItemResponse(Item item) : base(item)
        { }

        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public ItemResponse(string message) : base(message)
        { }
    }
}