using System.Collections.Generic;

namespace ShopProductService.Commands.Product
{
    public class FindProductsByCategoryIdQuery : FindProductsByKeywordQuery
    {
        public List<int> CategoryIds { get; set; }
    }
}
