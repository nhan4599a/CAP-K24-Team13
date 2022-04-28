using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.ViewComponents
{
    public class MobileMenuHeader : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string activeItem)
        {
            return Task.FromResult<IViewComponentResult>(View(model: activeItem));
        }
    }
}
