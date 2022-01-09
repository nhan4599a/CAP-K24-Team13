using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
    [Area("User")]
    public class ProductController : Controller
    {
        private readonly IProductClient _productClient;

        public ProductController(IProductClient productClient)
        {
            _productClient = productClient;
        }
        public IActionResult ListProduct()
        {
            return View();
        }

        public async Task<IActionResult> Index(string id)
        {
            var result = await _productClient.GetProductAsync(id);
            if (result.ResponseCode == 404)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            var product = result.Data;
            return View(JsonConvert.SerializeObject(product));
        }
    }
}