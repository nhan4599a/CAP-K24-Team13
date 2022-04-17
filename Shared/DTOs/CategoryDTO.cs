using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DTOs
{
    [NotMapped]
    public class CategoryDTO
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int ProductCount { get; set; }
    }
}
