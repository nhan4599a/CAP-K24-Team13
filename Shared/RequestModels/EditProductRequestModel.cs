namespace Shared.RequestModels
{
    public class EditProductRequestModel
    {
        public string ProductName { get; set; }

        public int CategoryId { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public int Discount { get; set; }

        public string[] ImagePaths { get; set; }
    }
}
