namespace ShopProductService.Commands.Product
{
    public class FindProductsOfShopInCategoryQuery : FindProductsByCategoryIdQuery
    {
        public int ShopId { get; set; }
    }
}
