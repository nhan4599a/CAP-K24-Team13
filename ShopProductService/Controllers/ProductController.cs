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


        public async Task<string> AddProduct(int CategoryId, string ProductName, string Description, int Quantity, double Price, int Discount)
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
            return "Submit Successfull";
        }
      
       
    }
    

}