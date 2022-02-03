using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestModels
{
    public class RatingRequestModel
    {
        public string ProductId { get; set; }

        public string UserId { get; set; }

        public string Message { get; set; }

        public int Star { get; set; }
    }
}
