using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiRESTv1.Models
{
    public class Item
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ForeignName { get; set; }
        public int ItemsGroupCode { get; set; }
        public string SalesVATGroup { get; set; }
        public string CodeBars { get; set; }
        public bool VatLiable { get; set; }
        public bool PurchaseItem { get; set; }
        public bool SalesItem { get; set; }
        public bool InventoryItem { get; set; }
        public string SupplierCatalogNo { get; set; }
        public string SalesUnit { get; set; }
        public double SalesItemsPerUnit { get; set; }
        public string Picture { get; set; }
        public double SalesUnitHeight { get; set; }
        public double SalesUnitVolume { get; set; }
        public List<Price> Prices { get; set; }
        public bool ManageSerialNumbers { get; set; }
        public bool ManageBatchNumbers { get; set; }
        public bool Valid { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string ValidRemarks { get; set; }
        public bool Frozen { get; set; }
        public DateTime FrozenFrom { get; set; }
        public DateTime FrozenTo { get; set; }
        public string FrozenRemarks { get; set; }
        public string U_GRUPO { get; set; }
        public string U_TIPOGRUPO { get; set; }
        public string U_TIPO { get; set; }
        public string U_SERIE { get; set; }
        public string U_COMPATIBILIDAD { get; set; }
        public string U_MEDIDAS { get; set; }
       
        public int UoMGroupEntry { get; set; }
        public string ClavProdServ { get; set; }
        public Properties Properties { get; set; }


        public UserFields UserFields { get; set; }
    }


    public class Properties
    {
        [JsonProperty("sample string 1")]
        public bool SampleString1 { get; set; }

        [JsonProperty("sample string 3")]
        public bool SampleString3 { get; set; }
    }

    public class UserFields
    {
        [JsonProperty("sample string 1")]
        public string SampleString1 { get; set; }

        [JsonProperty("sample string 3")]
        public string SampleString3 { get; set; }
    }
}
