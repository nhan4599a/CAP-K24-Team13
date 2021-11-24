using Microsoft.AspNetCore.Mvc;
using DatabaseAccessor.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DatabaseAccessor;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Controllers
{
    public class CategoryController : Controller
    {

      

        [ActionName("Add")]
        public IActionResult AddProduct()
        {
            
            var list = new List<string>() { "Computer", "Laptop", "Phone" };
            ViewBag.list = list;
            return View();
        } 
        [ActionName("Index")]
        public IActionResult ListProduct()
        {
            return View();
        }

        //@*@Html.DropDownListFor(model => Model.CategoryName, new SelectList(ViewBag.list))
        // @Html.ValidationMessageFor(model => model.CategoryName, "", new { @class = "text-danger" })*@
    }
}