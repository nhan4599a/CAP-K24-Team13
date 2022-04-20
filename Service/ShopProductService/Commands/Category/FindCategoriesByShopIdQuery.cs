namespace ShopProductService.Commands.Category
{
    public class FindCategoriesByShopIdQuery : FindAllCategoriesQuery
    {
        public int ShopId { get; set; }
    }
}
