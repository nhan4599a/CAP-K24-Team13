using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Models
{
    public class CreateUserModel : Controller
    {
        public string CategoryName { get; set; }

        public int Special { get; set; }

        public string ImagePath { get; set; }
    }
}
