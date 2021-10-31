namespace DatabaseAccessor.Model
{
    public class ProductImage
    {
        public int Id { get; set; }

        public int ShopProductId { get; set; }

        public string Image1 { get; set; }

        public string Image2 { get; set; }

        public string Image3 { get; set; }

        public string Image4 { get; set; }

        public string Image5 { get; set; }

        public virtual ShopProduct Product { get; set; }
    }
}
