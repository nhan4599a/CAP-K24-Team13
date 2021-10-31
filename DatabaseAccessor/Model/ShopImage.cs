using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccessor.Model
{
    public class ShopImage
    {
        public int Id { get; set; }

        public int ShopInterfaceId { get; set; }

        public string Image1 { get; set; }

        public string Image2 { get; set; }

        public string Image3 { get; set; }

        public string Image4 { get; set; }

        public string Image5 { get; set; }

        public string Image6 { get; set; }

        public string Image7 { get; set; }

        public string Image8 { get; set; }

        public string Image9 { get; set; }

        public string Image10 { get; set; }

        public string Image11 { get; set; }

        public string Image12 { get; set; }

        public virtual ShopInterface ShopInterface { get; set; }
    }
}
