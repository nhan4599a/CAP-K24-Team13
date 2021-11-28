using Microsoft.AspNetCore.Mvc;

namespace GUI.ViewComponents
{
    public class CategoryDropdown : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
