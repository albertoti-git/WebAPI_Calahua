using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiRESTv1.Models
{
	public class IncPayments
	{
        public string CardCode { get; set; }
        public int DocEntry { get; set; }
        public DateTime DocDate { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentFrm { get; set; }
        public string TransferRef { get; set; }
        public double TransferSum { get; set; }
        public string TransferAccount { get; set; }

        public string Comments { get; set; }

        public string Memos { get; set; }
        public Dictionary<string, string>[] UserFields { get; set; }

    }
}