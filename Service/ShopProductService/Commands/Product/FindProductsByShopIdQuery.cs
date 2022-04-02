namespace ShopProductService.Commands.Product
{
    public class FindProductsByShopIdQuery : FindProductsQuery
    {
        public int ShopId { get; set; }
    }
}
