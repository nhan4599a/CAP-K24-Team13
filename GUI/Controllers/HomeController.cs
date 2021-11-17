using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly INotyfService _notyf;

        public HomeController(ILogger<HomeController> logger,INotyfService notyf)
        {
          
        }
        public IActionResult Index()
        {
          
            return View();
        }
    }
}
