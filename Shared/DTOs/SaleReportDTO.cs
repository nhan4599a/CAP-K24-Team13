using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class SaleReportDTO
    {
        public List<ProductReportDTO> ProductsList { get; set; }
        public double Total { get; set; }
    }
}
