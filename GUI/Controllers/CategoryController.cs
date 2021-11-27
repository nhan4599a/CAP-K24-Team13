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
        private readonly HttpClient _client;

        public HttpClient Client => _client;
        public CategoryController(HttpClient client)
        {
            _client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ActionName("Detail")]
        public async Task<IActionResult> Detail(int id)
        {
            var response = await Client.GetAsync("https://localhost:44302/api/categories/" + id);
            response.EnsureSuccessStatusCode();
            var responseStream = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ApiResult<CategoryDTO>>(responseStream);
            if (data.Data != null)
                return View(data.Data);
            return View(data.Data);
        }

        [ActionName("Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await Client.GetAsync("https://localhost:44302/api/categories/" + id);
            response.EnsureSuccessStatusCode();
            var responseStream = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ApiResult<CategoryDTO>>(responseStream);
            var result = new CategoryDTO { };
            if (data.Data != null)
                result = data.Data;
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid) return View(ModelState);
            var response = await Client.PutAsJsonAsync("https://localhost:44302/api/categories/", categoryDTO);
            response.EnsureSuccessStatusCode();
            var responseStream = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ApiResult<bool>>(responseStream);
            if (data.Data)
                return RedirectToAction("Detail", new { id = categoryDTO.Id });
            return View(categoryDTO);
        }
    }
}
