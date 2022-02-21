using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GUI.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            var responseCode = HttpContext.Session.GetInt32("ResponseCode");
            if (responseCode.HasValue)
                HttpContext.Session.Remove("ResponseCode");
            responseCode = (!responseCode.HasValue || (responseCode != 404 && responseCode != 500)) ? 500 : responseCode;
            return View(responseCode.Value.ToString());
        }
    }
}
