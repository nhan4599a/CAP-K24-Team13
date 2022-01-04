using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestModels
{
    public class RemoveCartItemRequestModel
    {
        public string UserId { get; set; } = "69";
        public string ProductId { get; set; }
    }
}
