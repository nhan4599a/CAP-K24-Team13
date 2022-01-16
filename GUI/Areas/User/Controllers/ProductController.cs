using GUI.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
    [VirtualArea("User")]
    public class ProductController : Controller
    {
        private readonly IProductClient _productClient;

        public ProductController(IProductClient productClient)
        {
            _productClient = productClient;
        }

        public async Task<IActionResult> Detail(string id)
        {
            var response = await _productClient.GetProductAsync(id);
            if (!response.IsSuccessStatusCode || response.Content.ResponseCode != StatusCodes.Status200OK)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }
            return View(response.Content.Data);
        }
    }
}
