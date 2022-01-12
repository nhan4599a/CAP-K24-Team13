using GUI.Areas.Customer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared;
using Shared.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GUI.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string serverUrl = "https://localhost:44394/api/";
        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Search([FromForm] string keyword)
        {
            var response = await _httpClient.GetAsync(serverUrl + "interfaces?searchString=" + keyword);

            if (!response.IsSuccessStatusCode)
            {
                return View();
            };
            var responseStream = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ApiResult<List<ShopViewModel>>>(responseStream);
            if (data.Data == null)
            {
                return View();
            }
            return View(data.Data);
        }
    }
}
