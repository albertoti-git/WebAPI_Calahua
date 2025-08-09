using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiRESTv1.Models
{
    public class Address
    {
        public string AddressName { get; set; }
        public string Street { get; set; }
        public string Block { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string FederalTaxID { get; set; }
        public string TaxCode { get; set; }
        public string BuildingFloorRoom { get; set; }
        public string AddressType { get; set; }
        public string StreetNo { get; set; }
        public string GlobalLocationNumber { get; set; }
    }
}