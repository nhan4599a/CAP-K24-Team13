namespace ShopProductService.Commands.Product
{
    public class FindProductsByKeywordQuery : FindProductsQuery
    {
        public string Keyword { get; set; }
    }
}
