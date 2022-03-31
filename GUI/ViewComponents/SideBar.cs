using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.ViewComponents
{
    public class SideBar : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string activeItem)
        {
            return Task.FromResult<IViewComponentResult>(View(model: activeItem));
        }
    }
}
