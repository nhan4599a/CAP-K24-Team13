using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared;
using Shared.DTOs;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _client;

        public HttpClient Client => _client;

        public ProductController(HttpClient client)
        {
            _client = client;
        }

        [ActionName("Detail")]
        public async Task<IActionResult> DetailProduct(string id)
        {
            var response = await Client.GetAsync("https://localhost:44302/api/products/" + id);
            response.EnsureSuccessStatusCode();
            var responseStream = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ApiResult<ProductDTO>>(responseStream);
            if (data.Data != null)
                return View(data.Data);
            return View(data.Data);
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
        [ActionName("Edit")]
        public async Task<IActionResult> EditProduct(string id)
        {
            var response = await Client.GetAsync("https://localhost:44302/api/products/" + id);
            response.EnsureSuccessStatusCode();
            var responseStream = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ApiResult<ProductDTO>>(responseStream);
            var productDto = new ProductDTO { };
            if (data.Data != null)
                return View(data.Data);
            return View(data.Data);
        }
        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var response = await Client.PutAsJsonAsync("https://localhost:44302/api/products/", productDTO);
            response.EnsureSuccessStatusCode();
            var responseStream = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ApiResult<bool>>(responseStream);
            if (data.Data)
                return RedirectToAction("Detail", new {id = productDTO.Id});
            return View(productDTO);
        }
    }
}
