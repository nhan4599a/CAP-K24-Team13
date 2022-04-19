using System;

namespace Shared.DTOs
{
    public class ReportDTO
    {
        public int Id { get; set; }

        public string Reporter { get; set; }

        public string AffectedUser { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
