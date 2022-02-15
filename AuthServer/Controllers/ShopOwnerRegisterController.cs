using AuthServer.Identities;
using AuthServer.Models;
using DatabaseAccessor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using System.Threading.Tasks;

namespace AuthServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/Account/Shopowner")]
    public class ShopOwnerRegisterController : ControllerBase
    {
        private readonly ApplicationUserManager _userManager;

        private readonly IMailService _mailService;

        public ShopOwnerRegisterController(ApplicationUserManager userManager, IMailService mailService)
        {
            _userManager = userManager;
            _mailService = mailService;
        }

        [HttpPost]
        public async Task<CreateShopOwnerAccountResponse> ShopOwnerRegister(CreateShopOwnerAccountModel model)
        {
            var user = new User
            {
                UserName = $"{model.PhoneNumber}.{model.Location}",
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                DoB = model.DoB,
                NormalizedUserName = $"{model.PhoneNumber}.{model.Location}".ToUpper(),
                NormalizedEmail = model.Email.ToUpper()
            };

            var createUserResult = await _userManager.CreateAsync(user);

            if (createUserResult.Succeeded)
            {
                var addToRoleResult = await _userManager.AddToRoleAsync(user, Roles.SHOP_OWNER);
                if (addToRoleResult.Succeeded)
                    return CreateShopOwnerAccountResponse.CreateSuccessResult($"{model.PhoneNumber}.{model.Location}");
                return CreateShopOwnerAccountResponse.CreateFailedResponse(addToRoleResult.Errors);
            }
            return CreateShopOwnerAccountResponse.CreateFailedResponse(createUserResult.Errors);
        }
    }
}
