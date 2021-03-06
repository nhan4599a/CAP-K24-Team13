using GUI.Abtractions;
using GUI.Attributes;
using GUI.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
    [VirtualArea("User")]
    public class ProductController : BaseUserController
    {
        private readonly IProductClient _productClient;

        public ProductController(IProductClient productClient, ICartClient cartClient)
        {
            _productClient = productClient;
        }

        public async Task<IActionResult>Index(string id)
        {
        	var productResponse = await _productClient.GetProductAsync(id);
        	if (!productResponse.IsSuccessStatusCode || productResponse.Content.ResponseCode != StatusCodes.Status200OK)
			    return new StatusCodeResult(StatusCodes.Status404NotFound);
		    return View(productResponse.Content.Data);
	    }
    }
}
