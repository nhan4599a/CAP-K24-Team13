using GUI.Abtractions;
using Microsoft.AspNetCore.Mvc;

namespace GUI.Areas.Admin.Controllers
{
    public class ReportController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
