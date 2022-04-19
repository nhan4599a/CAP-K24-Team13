using System;

namespace Shared.DTOs
{
    public class RatingDTO
    {
        public string Id { get; set; }

        public string ProductName { get; set; }

        public string UserName { get; set; }

        public string Message { get; set; }

        public int Star { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
