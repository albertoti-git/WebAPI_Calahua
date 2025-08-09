using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiRESTv1.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SAPbobsCOM;
using WebApiRESTv1.Repositories;
using System.Threading.Tasks;

namespace WebApiRESTv1.Controllers
{
    [Authorize]
    public class QuotationController : ApiController
    {
        private readonly IOrderDiApiRepository _orderRepository;
        private readonly IQuoteRepository _quoteRepository;


        public QuotationController(IOrderDiApiRepository orderRepository, IQuoteRepository quoteRepository )
        {
            _orderRepository = orderRepository;
            _quoteRepository = quoteRepository;
        }
        string strConection = ConfigurationManager.AppSettings.Get("bdcon");
        public IHttpActionResult Get(int DocEntry)
        {


            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(strConection))
            {
                String sql = string.Format("Sp_AYB_WebAPI {0}, '{1}'", 1, DocEntry);
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter sqlDA;
                connection.Open();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                sqlDA = new SqlDataAdapter(cmd);
                sqlDA.Fill(dt);

            }
            int ColCount = dt.Columns.Count;
            int RowCount = dt.Rows.Count;
            string ITEMS = string.Empty;
            ITEMS += "[";
            for (int j = 0; j < RowCount - 1; j++)
            {
                string ITEM = string.Empty;
                ITEM = "{";
                string ITEMVALUE = string.Empty;
                for (int i = 0; i < ColCount - 1; i++)
                {
                    string ColName = dt.Columns[i].ColumnName;
                    ITEMVALUE = dt.Rows[j][i].ToString();
                    Type tipoDato = dt.Rows[j][i].GetType();
                    if (tipoDato.Name == "String")
                    {
                        ITEM += "\"" + ColName + "\"" + " : " + "\"" + ITEMVALUE + "\"";
                    }
                    else
                    {
                        ITEM += "\"" + ColName + "\"" + " : " + ITEMVALUE;
                    }

                    ITEM += ",";
                }
                if (ITEMVALUE.Length > 0)
                {
                    ITEM = ITEM.Substring(0, ITEM.Length - 1);
                }
                ITEM += "},";
                ITEMS += ITEM;
            }
            ITEMS = ITEMS.Substring(0, ITEMS.Length - 1);
            ITEMS += "]";

            dynamic json = JsonConvert.DeserializeObject(ITEMS);
            return Ok(json);
        }

        //public IHttpActionResult post(Quotation quotation)
        //{
        //    string str_sp = "pa_insertaquotation";

        //    string SQL = string.Format("{0} {1},{2},{3},'{4}','{5}','{6}','{7}','{8}','{9}','{10}',{11},{12},{13},{14},'{15}','{16}','{17}','{18}','{19}',{20},'{21}','{22}','{23}'", str_sp,
        //        quotation.DocEntry,quotation.DocNum,quotation.Series,
        //        quotation.FederalTaxID,quotation.DocDate,quotation.DocDueDate,quotation.CardCode,quotation.CardName,quotation.NumAtCard,quotation.DocCurrency,quotation.DocRate,quotation.DocTotal,quotation.PaidToDate,
        //        quotation.VatSum,quotation.DocStatus,quotation.Comments,quotation.FolioPOS,quotation.SecuenciaPOS,quotation.TaxDate,quotation.DiscountPercent,quotation.Project,quotation.SeriesString,quotation.TaxInvoiceDate);

        //    using (SqlConnection connection = new SqlConnection(strConection))
        //    {
        //        SqlCommand cmd = new SqlCommand();
        //        connection.Open();
        //        cmd.CommandText = SQL;
        //        cmd.CommandType = CommandType.Text;
        //        cmd.Connection = connection;
        //        try
        //        {
        //            int quo_id = Convert.ToInt32(cmd.ExecuteScalar());

        //            foreach(Line linea in quotation.Lines)
        //            {
        //                string sql2 = string.Format("pa_insertalineas {0}, '{1}','{1}',{2},{3},{4},{5},'{6}',{7},{8},{9},{10},{11},{12},{13},'{14}','{15}'", quo_id,
        //                linea.ItemCode, linea.ItemDescription, linea.OpenQuantity, linea.Quantity, linea.Price,
        //                linea.PriceAfterVAT, linea.TaxCode, linea.DiscountPercent, linea.BaseEntry, linea.BaseType, linea.LineNum, linea.BaseLineNum, linea.TaxPercentage, linea.LineTotal, linea.BarCode, linea.WarehouseCode);
        //                cmd.CommandText = sql2;
        //                cmd.ExecuteNonQuery();
        //            }
        //            foreach(Address adress in quotation.Addresses)
        //            {
        //                string sql3 = string.Format("pa_insertadirecciones {0}, '{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}'", quo_id,
        //                    adress.Street,adress.Block,adress.ZipCode,adress.City,adress.County,
        //                    adress.Country,adress.State,adress.FederalTaxID,adress.TaxCode,adress.BuildingFloorRoom,adress.AddressType,adress.StreetNo,adress.GlobalLocationNumber);
        //                cmd.CommandText = sql3;
        //                cmd.ExecuteNonQuery();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return BadRequest(ex.Message);
        //        }

        //    }
        //    return Ok();
        //}

        public HttpResponseMessage Post([FromBody] Quotation quotation)
        {
            ConexionSAP conexionSAP = ConexionSAP.GetInstance;
            Documents documents = (Documents)(dynamic)conexionSAP.CompanySBO.GetBusinessObject(BoObjectTypes.oQuotations);
            Recordset recordset = (Recordset)(dynamic)conexionSAP.CompanySBO.GetBusinessObject(BoObjectTypes.BoRecordset);
            documents.CardCode = quotation.CardCode;
            documents.CardName = quotation.CardName;
            if (!string.IsNullOrEmpty(quotation.FederalTaxID))
            {
                documents.FederalTaxID = quotation.FederalTaxID;
            }
            if (!string.IsNullOrEmpty(quotation.DocCurrency))
            {
                documents.DocCurrency = quotation.DocCurrency;
            }
            if (quotation.DocRate > 0.0)
            {
                documents.DocDate = quotation.DocDate;
            }
            documents.DocRate = quotation.DocRate;
            if (quotation.DocTotal > 0.0)
            {
                documents.DocTotal = quotation.DocTotal;
            }
            documents.DocDueDate = quotation.DocDueDate;
            documents.ContactPersonCode = quotation.ContactPersonCode;
            documents.NumAtCard = quotation.NumAtCard;
            if (!string.IsNullOrEmpty(quotation.Project))
            {
                documents.Project = quotation.Project;
            }
            //documents.UserFields.Fields.Item("U_CVM_ORIGENPOS").Value = "Y";
            //documents.UserFields.Fields.Item("U_CVM_FOLIOPOS").Value = quotation.FolioPOS;
            //documents.UserFields.Fields.Item("U_CVM_SECPOS").Value = quotation.SecuenciaPOS;
            if (quotation.Series > 0)
            {
                documents.Series = quotation.Series;
            }
            documents.TaxDate = quotation.TaxDate;
            for (int iIndex = 0; iIndex < quotation.Lines.Count(); iIndex++)
            {
                if (iIndex > 0)
                {
                    documents.Lines.Add();
                }
                documents.Lines.ItemCode = quotation.Lines[iIndex].ItemCode;
                documents.Lines.ItemDescription = quotation.Lines[iIndex].ItemDescription;
                documents.Lines.Quantity = quotation.Lines[iIndex].Quantity;
                documents.Lines.UnitPrice = quotation.Lines[iIndex].Price;
                documents.Lines.TaxCode = quotation.Lines[iIndex].TaxCode;
                documents.Lines.TaxPercentagePerRow = quotation.Lines[iIndex].TaxPercentage;
                documents.Lines.DiscountPercent = quotation.Lines[iIndex].DiscountPercent;
                if (!string.IsNullOrEmpty(quotation.Lines[iIndex].WarehouseCode))
                {
                    documents.Lines.WarehouseCode = quotation.Lines[iIndex].WarehouseCode;
                }
                if (quotation.Lines[iIndex].UserFields == null || quotation.Lines[iIndex].UserFields.Count <= 0)
                {
                    continue;
                }
                foreach (string sCampo in quotation.Lines[iIndex].UserFields.Keys)
                {
                    documents.Lines.UserFields.Fields.Item(sCampo).Value = quotation.Lines[iIndex].UserFields[sCampo];
                }
            }
            if (quotation.UserFields != null && quotation.UserFields.Count() > 0)
            {
                foreach (string sCampo2 in quotation.UserFields.Keys)
                {
                    documents.UserFields.Fields.Item(sCampo2).Value = quotation.UserFields[sCampo2];
                }
            }
            if (documents.Add() != 0)
            {
                HttpResponseMessage response = base.Request.CreateResponse(HttpStatusCode.BadRequest, quotation);
                response.ReasonPhrase = conexionSAP.CompanySBO.GetLastErrorDescription();
                return base.Request.CreateResponse(HttpStatusCode.InternalServerError, conexionSAP.CompanySBO.GetLastErrorDescription());
                // return response;
            }
            string sDocEntry = conexionSAP.CompanySBO.GetNewObjectKey();
            quotation.DocEntry = int.Parse(sDocEntry);
            quotation = Get_Resp(int.Parse(sDocEntry));
            return base.Request.CreateResponse(HttpStatusCode.Created, quotation);
        }

        private Quotation Get_Resp(int DocEntry)
        {
            Quotation invoice = new Quotation();
            try
            {
                ConexionSAP conexionSAP = ConexionSAP.GetInstance;
                Documents documents = (Documents)(dynamic)conexionSAP.CompanySBO.GetBusinessObject(BoObjectTypes.oQuotations);
                List<Quotation> oListaSocios = new List<Quotation>();
                if (documents.GetByKey(DocEntry))
                {
                    invoice.CardCode = documents.CardCode;
                    invoice.CardName = documents.CardName;
                    invoice.FederalTaxID = documents.FederalTaxID;
                    invoice.DocCurrency = documents.DocCurrency;
                    invoice.DocDate = documents.DocDate;
                    invoice.DocEntry = documents.DocEntry;
                    invoice.DocNum = documents.DocNum;
                    invoice.DocRate = documents.DocRate;
                    invoice.DocTotal = documents.DocTotal;
                    invoice.DocDueDate = documents.DocDueDate;
                    invoice.NumAtCard = documents.NumAtCard;
                    invoice.Project = documents.Project;
                    invoice.Series = documents.Series;
                    invoice.SeriesString = documents.SeriesString;
                    invoice.TaxDate = documents.TaxDate;
                    invoice.Comments = documents.Comments;
                    invoice.Lines = new Line[documents.Lines.Count];
                    for (int iIndex2 = 0; iIndex2 < documents.Lines.Count; iIndex2++)
                    {
                        documents.Lines.SetCurrentLine(iIndex2);
                        invoice.Lines[iIndex2] = new Line();
                        invoice.Lines[iIndex2].ItemCode = documents.Lines.ItemCode;
                        invoice.Lines[iIndex2].ItemDescription = documents.Lines.ItemDescription;
                        invoice.Lines[iIndex2].BarCode = documents.Lines.BarCode;
                        invoice.Lines[iIndex2].Quantity = documents.Lines.Quantity;
                        invoice.Lines[iIndex2].Price = documents.Lines.UnitPrice;
                        invoice.Lines[iIndex2].TaxCode = documents.Lines.TaxCode;
                        invoice.Lines[iIndex2].TaxPercentage = documents.Lines.TaxPercentagePerRow;
                        invoice.Lines[iIndex2].DiscountPercent = documents.Lines.DiscountPercent;
                        invoice.Lines[iIndex2].WarehouseCode = documents.Lines.WarehouseCode;
                        invoice.Lines[iIndex2].UserFields = new Dictionary<string, string>();
                        for (int iIndex3 = 0; iIndex3 < documents.Lines.UserFields.Fields.Count; iIndex3++)
                        {
                            if (!invoice.Lines[iIndex2].UserFields.ContainsKey(documents.Lines.UserFields.Fields.Item(iIndex3).Name))
                            {
                                invoice.Lines[iIndex2].UserFields.Add(documents.Lines.UserFields.Fields.Item(iIndex3).Name, ((dynamic)documents.Lines.UserFields.Fields.Item(iIndex3).Value).ToString());
                            }
                        }
                    }
                    invoice.UserFields = new Dictionary<string, string>();
                    for (int iIndex = 0; iIndex < documents.UserFields.Fields.Count; iIndex++)
                    {
                        if (!invoice.UserFields.ContainsKey(documents.UserFields.Fields.Item(iIndex).Name))
                        {
                            invoice.UserFields.Add(documents.UserFields.Fields.Item(iIndex).Name, ((dynamic)documents.UserFields.Fields.Item(iIndex).Value).ToString());
                        }
                    }
                    invoice.Addresses = new Address[2];
                    invoice.Addresses[0] = new Address();
                    invoice.Addresses[0].AddressName = documents.PayToCode;
                    invoice.Addresses[0].AddressType = documents.AddressExtension.BillToAddressType.ToString();
                    invoice.Addresses[0].Street = documents.AddressExtension.BillToStreet;
                    invoice.Addresses[0].Block = documents.AddressExtension.BillToBlock;
                    invoice.Addresses[0].ZipCode = documents.AddressExtension.BillToZipCode;
                    invoice.Addresses[0].City = documents.AddressExtension.BillToCity;
                    invoice.Addresses[0].County = documents.AddressExtension.BillToCounty;
                    invoice.Addresses[0].Country = documents.AddressExtension.BillToCountry;
                    invoice.Addresses[0].State = documents.AddressExtension.BillToState;
                    invoice.Addresses[0].BuildingFloorRoom = documents.AddressExtension.BillToBuilding;
                    invoice.Addresses[0].StreetNo = documents.AddressExtension.BillToStreetNo;
                    invoice.Addresses[0].GlobalLocationNumber = documents.AddressExtension.BillToGlobalLocationNumber;
                    invoice.Addresses[1] = new Address();
                    invoice.Addresses[1].AddressName = documents.ShipToCode;
                    invoice.Addresses[1].AddressType = documents.AddressExtension.ShipToAddressType.ToString();
                    invoice.Addresses[1].Street = documents.AddressExtension.ShipToStreet;
                    invoice.Addresses[1].Block = documents.AddressExtension.ShipToBlock;
                    invoice.Addresses[1].ZipCode = documents.AddressExtension.ShipToZipCode;
                    invoice.Addresses[1].City = documents.AddressExtension.ShipToCity;
                    invoice.Addresses[1].County = documents.AddressExtension.ShipToCounty;
                    invoice.Addresses[1].Country = documents.AddressExtension.ShipToCountry;
                    invoice.Addresses[1].State = documents.AddressExtension.ShipToState;
                    invoice.Addresses[1].BuildingFloorRoom = documents.AddressExtension.ShipToBuilding;
                    invoice.Addresses[1].StreetNo = documents.AddressExtension.ShipToStreetNo;
                    invoice.Addresses[1].GlobalLocationNumber = documents.AddressExtension.ShipToGlobalLocationNumber;
                    documents.Browser.MoveNext();
                }
            }
            catch (Exception)
            {
            }
            return invoice;
        }

        [Route("Quotation/SaveOrder")]
        public IHttpActionResult Save([FromBody] List<PedidoRequest> pedidos)
        {
            var username = User.Identity?.Name ?? "Desconocido";

            var response = _orderRepository.GuardarPedidos(pedidos, username, BoObjectTypes.oQuotations);
            return Ok(response);
        }
        [HttpGet]
        [Route("api/quotation/get")]
        public async Task<IHttpActionResult> GetQuotations([FromUri] string docEntry = null, string cardCode = null)
        {
            if (string.IsNullOrWhiteSpace(docEntry) && string.IsNullOrWhiteSpace(cardCode))
            {
                return BadRequest("Debe proporcionar al menos un parámetro: docEntry o cardCode.");
            }
            var orders = await _quoteRepository.GetQuotationsAsync(docEntry, cardCode);
            return Ok(orders);
        }

    }
}
