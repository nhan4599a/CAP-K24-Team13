using DatabaseAccessor;
using DatabaseAccessor.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopProductService.Controllers
{
    public class Category : Controller
    {
        [ApiController]

        [Route("/api/categoies")]

        public class ProductController : Controller
        {
            private ApplicationDbContext _dbContext;

            public ProductController(ApplicationDbContext dbcontext)
            {
                _dbContext = dbcontext;
            }

            [ActionName("Add")]


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
}
  