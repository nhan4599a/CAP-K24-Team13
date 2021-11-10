using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAccessor;
using DatabaseAccessor.Model;

namespace ShopApplication.Product
{
    public class AddProduct
    {
        private ApplicationDbContext _dbContext;
        public AddProduct(ApplicationDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }
        public async Task DoAsync(string id, int CategoryId, string ProductName, string Description, int Quantity, double Price, int Discount)
        {
            _dbContext.ShopProducts.Add(new DatabaseAccessor.Model.ShopProduct
            {
                Id = id,
                CategoryId = CategoryId,
                ProductName = ProductName,
                Description = Description,
                Quantity = Quantity,
                Price = Price,
                Discount = Discount,

            });

            await _dbContext.SaveChangesAsync();
        }

    }
}
