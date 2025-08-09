using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiRESTv1.Models
{
    public class Price
    {
        public string ItemCode { get; set; }
        public int PriceList { get; set; }
        public string PriceListName { get; set; }
        public double Precio { get; set; }
        public string Currency { get; set; }
        public bool IsGrossPrc { get; set; }
    }
}