

using Shared;

namespace AuthServer.Models
{
    public class CreateOrEditUserRequestModel : IRequest<CommandResponse<Guid>>
    {
        public CreateOrEditUserRequestModel RequestModel { get; set; }

        public static implicit operator CreateOrEditUserRequestModel(SignUpModel v)
        {
            throw new NotImplementedException();
        }
    }
}
