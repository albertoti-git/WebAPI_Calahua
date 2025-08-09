using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiRESTv1.Models
{
    public class Quotation
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public int Series { get; set; }
        public string FederalTaxID { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string NumAtCard { get; set; }
        public string DocCurrency { get; set; }
        public double DocRate { get; set; }
        public double DocTotal { get; set; }
        public double PaidToDate { get; set; }
        public double VatSum { get; set; }
        public string DocStatus { get; set; }
        public string Comments { get; set; }
        public string FolioPOS { get; set; }
        public string SecuenciaPOS { get; set; }

        public int ContactPersonCode { get; set; }
        public DateTime TaxDate { get; set; }
        //public List<Line> Lines { get; set; }
        public Line[] Lines { get; set; }
        //   public UserFields UserFields { get; set; }
        public Dictionary<string, string> UserFields { get; set; }
        public double DiscountPercent { get; set; }
        public string Project { get; set; }
        public string SeriesString { get; set; }
        public DateTime TaxInvoiceDate { get; set; }
     //   public List<Address> Addresses { get; set; }
        public Address[] Addresses { get; set; }
    }
}