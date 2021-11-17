
using AspNetCoreHero.ToastNotification.Abstractions;
using DatabaseAccessor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace GUI.Controllers
{
   
    public class ProductController : Controller
    {

        


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