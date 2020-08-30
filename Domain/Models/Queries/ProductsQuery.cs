namespace Product.API.Domain.Models.Queries
{
    public class ProductsQuery : Query
    {
        public int? ItemId { get; set; }

        public ProductsQuery(int? itemId, int page, int itemsPerPage) : base(page, itemsPerPage)
        {
            ItemId = itemId;
        }
    }
}