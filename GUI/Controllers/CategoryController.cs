using DatabaseAccessor;
using DatabaseAccessor.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GUI.Controllers
{
    public class CategoryController : Controller
    {
        private ApplicationDbContext _dbContext;

        public CategoryController(ApplicationDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        [ActionName("Add")]
        public IActionResult AddProduct()
        {
            return View();
        } 

        [ActionName("Index")]
        public IActionResult ListProduct()
        {
            return View();
        }
    }
}
