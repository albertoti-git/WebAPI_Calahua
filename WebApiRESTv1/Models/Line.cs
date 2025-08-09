using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiRESTv1.Models
{
    public class Line
    {
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public double OpenQuantity { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double PriceAfterVAT { get; set; }
        public string TaxCode { get; set; }
        public double DiscountPercent { get; set; }
        public int BaseEntry { get; set; }
        public int BaseType { get; set; }
        public int LineNum { get; set; }
        public int BaseLineNum { get; set; }
        public double TaxPercentage { get; set; }
        public double LineTotal { get; set; }
        public string BarCode { get; set; }
        public string WarehouseCode { get; set; }
        //  public UserFields UserFields { get; set; }
        public Dictionary<string, string> UserFields { get; set; }
    }
}