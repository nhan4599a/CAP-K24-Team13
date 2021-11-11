namespace DatabaseAccessor.Model
{

    public class ShopProduct 
    {
        
        public string Id { get; set; }

        public int CategoryId { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public int Discount { get; set; }

        public bool IsDisabled { get; set; }

        public virtual ShopCategory Category { get; set; }
        
        public virtual ProductImage ImageSet { get; set; }
    }
}
