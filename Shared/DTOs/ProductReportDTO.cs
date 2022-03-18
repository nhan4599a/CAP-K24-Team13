using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class ProductReportDTO
    {
        public string ProductName { get; set; }

        public Guid ProductId { get; set; }

        public DateTime Date { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public double Subtotal { get; set; }
    }
}
