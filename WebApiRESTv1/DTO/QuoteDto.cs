using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiRESTv1.DTO
{
    public class QuoteDto
    {
        public int DocEntry { get; set; }
        public DateTime? DocDate { get; set; }
        public string DocStatus { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public DateTime? DocDueDate { get; set; }
        public string DocCur { get; set; }
        public decimal DocRate { get; set; }
        public string NumAtCard { get; set; }
        public int CntctCode { get; set; }
        public string Comments { get; set; }
        public short GroupNum { get; set; }
        public string ShipToCode { get; set; }
        public string U_B1SYS_MainUsage { get; set; }
        public decimal DocTotal { get; set; }
        public decimal VatSum { get; set; }
        public decimal PaidToDate { get; set; }

        public List<QuoteDetailDto> QuoteDetails { get; set; }
    }
    public class QuoteDetailDto
    {
        public int DocEntry { get; set; }
        public string LineStatus { get; set; }
        public int LineNum { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public decimal Quantity { get; set; }
        public decimal OpenQty { get; set; }
        public decimal Price { get; set; }
        public decimal PriceBefDi { get; set; }
        public decimal LineTotal { get; set; }
        public decimal VatSum { get; set; }
        public decimal VatPrcnt { get; set; }
        public decimal OpenSum { get; set; }
    }

}