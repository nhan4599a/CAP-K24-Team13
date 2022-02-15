using AuthServer.Models;

namespace AuthServer.Controllers
{
    internal interface IMediator
    {
        Task Send(CreateOrEditUserRequestModel createOrEditUserRequestModel);
    }
}