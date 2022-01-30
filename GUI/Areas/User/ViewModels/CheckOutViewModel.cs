using Shared.DTOs;

namespace GUI.Areas.User.ViewModels
{
    public class CheckOutViewModel
    {
        public int Quantity { get; set; }

        public MinimalProductDTO Item { get; set; }
    }
}
