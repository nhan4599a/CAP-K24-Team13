using Microsoft.AspNetCore.Mvc;

namespace GUI.ViewComponents
{
    public class PageSizeSelectDropdown : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
