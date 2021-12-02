using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class ShopInterfaceDTO
    {
        public int Id { get; set; }

        public int ShopId { get; set; }

        public int Option { get; set; }

        public string Images { get; set; }

        public string Description { get; set; }
    }
}
