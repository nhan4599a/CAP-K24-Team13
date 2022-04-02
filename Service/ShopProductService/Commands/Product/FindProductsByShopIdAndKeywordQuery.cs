namespace ShopProductService.Commands.Product
{
    public class FindProductsByShopIdAndKeywordQuery : FindProductsByShopIdQuery
    {
        public string Keyword { get; set; }
    }
}
