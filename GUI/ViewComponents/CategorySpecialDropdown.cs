using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.ViewComponents
{
    public class CategorySpecialDropdown : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync()
        {
            return Task.FromResult<IViewComponentResult>(View());
        }
    }
}
