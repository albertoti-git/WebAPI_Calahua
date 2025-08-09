using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiRESTv1.Models
{
    public class BusinessPartner
    {
        public string CardCode { get; set; }
        public string CardType { get; set; }
        public string CardName { get; set; }
        public int GroupCode { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string MailAddress { get; set; }
        public string MailZipCode { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Fax { get; set; }
        public string ContactPerson { get; set; }
        public int PayTermsGrpCode { get; set; }
        public double CreditLimit { get; set; }
        public double DiscountPercent { get; set; }
        public string FederalTaxID { get; set; }
        public int PriceListNum { get; set; }
        public string FreeText { get; set; }
        public int SalesPersonCode { get; set; }
        public string Currency { get; set; }
        public string Cellular { get; set; }
        public int AvarageLate { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string MailCity { get; set; }
        public string MailCounty { get; set; }
        public string MailCountry { get; set; }
        public string EmailAddress { get; set; }
        public string Picture { get; set; }
        public string AdditionalID { get; set; }
        public string FatherCard { get; set; }
        public string CardForeignName { get; set; }
        public double CurrentAccountBalance { get; set; }
        public double OpenDeliveryNotesBalance { get; set; }
        public double OpenOrdersBalance { get; set; }
        public bool Valid { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool Frozen { get; set; }
        public DateTime FrozenFrom { get; set; }
        public DateTime FrozenTo { get; set; }
        public string Block { get; set; }
        public string ProjectCode { get; set; }
        public int Series { get; set; }
        public string GlobalLocationNumber { get; set; }
        public string UnifiedFederalTaxID { get; set; }
        public string UsoCFDI { get; set; }
        public ContactEmployees ContactEmployees { get; set; }
        // public UserFields UserFields { get; set; }
        public Dictionary<string, string> UserFields { get; set; }
        public BPAddress[] Addresses { get; set; }
    }
}