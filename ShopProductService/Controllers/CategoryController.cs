using DatabaseAccessor;
using DatabaseAccessor.Model;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopProductService.Controllers
{

    [ApiController]
    [Route("/api/categories")]
    public class CategoryController : Controller
    {
        private ApplicationDbContext _dbContext;

        public CategoryController(ApplicationDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<ApiResult<bool>> AddCategory(int ShopId, string CategoryName, int Special)
        {
            _dbContext.ShopCategories.Add(new ShopCategory
            {
                ShopId = ShopId,
                CategoryName = CategoryName,
                Special = Special,

            });

            await _dbContext.SaveChangesAsync();
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }

        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<ShopCategory> GetAllCategory()
        {
            return ShopCategoryList();
        }

        [NonAction]
        public List<ShopCategory> ShopCategoryList()
        {
            var cat = new List<ShopCategory>()
                {
                    new ShopCategory() {Id=1,ShopId=1,CatergoryName="Computer",Special=50000},
                    new ShopCategory() {Id=2,ShopId=1,CatergoryName="Laptop",Special=40000},
                    new ShopCategory() {Id=3,ShopId=1,CatergoryName="Phone",Special=30000},
                };
            return cat;
        }

        [HttpGet]
        [Route("GetByID/{catID}")]
        public ShopCategory GetCategoryByID(int catID)
        {
            return ShopCategoryList().SingleOrDefault(c => c.Id == catID);
        }
    }
}