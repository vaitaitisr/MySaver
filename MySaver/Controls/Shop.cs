using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySaver.Controls
{
    public class Shop
    {
        public string title { get; set; }
        public string address { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string postalCode { get; set; }
        public string website { get; set; }
        public Location location { get; set; }

        public class Location
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }
    }
}
