using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared;
using Shared.DTOs;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GUI.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ActionName("Edit")]
        public IActionResult Edit()
        {
            return View();
        }
    }
}
