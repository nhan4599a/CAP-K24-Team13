using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using System.Threading.Tasks;

namespace GUI.ViewComponents
{
    public class OrderItem : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(OrderDTO order)
        {
            return Task.FromResult<IViewComponentResult>(View(order));
        }
    }
}
