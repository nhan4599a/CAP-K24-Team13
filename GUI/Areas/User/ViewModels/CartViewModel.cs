using System.Collections.Generic;

namespace GUI.Areas.User.ViewModels
{
    public class CartViewModel
    {
        public string Email { get; set; }

        public int Size { get; set; }

        public List<object> Items { get; set; }
    }
}
