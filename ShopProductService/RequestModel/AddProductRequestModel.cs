namespace ShopProductService.RequestModel
{
    public class AddProductRequestModel
    {
        public string ProductName { get; set; }

        public int CategoryId { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public int Discount { get; set; }
    }
}
