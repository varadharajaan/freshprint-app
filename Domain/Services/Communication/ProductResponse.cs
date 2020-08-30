
namespace Product.API.Domain.Services.Communication
{
    public class ProductResponse : BaseResponse<Models.Product>
    {
        public ProductResponse(Models.Product product) : base(product) { }

        public ProductResponse(string message) : base(message) { }
    }
}