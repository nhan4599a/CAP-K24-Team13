using DatabaseAccessor;
using DatabaseAccessor.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ShopProductService.Controllers
{
    [ApiController]

    [Route("/api/products")]
    
    public class ProductController : Controller
    {
        private ApplicationDbContext _dbContext;

        public ProductController(ApplicationDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        [ActionName("Add")]


        public async Task AddProduct(int CategoryId, string ProductName, string Description, int Quantity, double Price, int Discount)
        {
            _dbContext.ShopProducts.Add(new ShopProduct
            {
                CategoryId = CategoryId,
                ProductName = ProductName,
                Description = Description,
                Quantity = Quantity,
                Price = Price,
                Discount = Discount,

            });

            await _dbContext.SaveChangesAsync();

        }
        [Route("/api/categoies")]
       
        public async Task AddCategories(int ShopId, string CategoryName, int Special)
        {
            _dbContext.ShopCategories.Add(new ShopCategory

            {
                ShopId = ShopId,
                CategoryName = CategoryName,
                Special = Special,

            });

            await _dbContext.SaveChangesAsync();

        }

    }


}