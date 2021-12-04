using Microsoft.AspNetCore.Mvc;

namespace GUI.ViewComponents
{
    public class CategorySpecialDropdown : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
