using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using WebApiRESTv1.Models;
using WebApiRESTv1.Repositories;
using System.Configuration;


namespace WebApiRESTv1.Controllers
{
    [Authorize]
    [RoutePrefix("api/prices")]
    public class PriceController : ApiController
    {
        private readonly IPriceRepository _repository;
        string strConection = ConfigurationManager.AppSettings.Get("bdcon");
        public PriceController(IPriceRepository repository)
        {
            _repository = repository;
        }

        //[HttpGet]
        //[Route("GetPrices")]
        //public async Task<IHttpActionResult> GetPrices([FromUri] string cardCode = null, string itemCode = null)
        //{
        //    if (string.IsNullOrWhiteSpace(cardCode))
        //    {
        //        return BadRequest("Debe proporcionar el parámetro cardCode.");
        //    }
        //    var result = await _repository.GetPricesAsync(cardCode, itemCode);
        //    return Ok(result);
        //}

        [Route("GetPrices")]
        //  public IHttpActionResult GetItemPage(PageParameter pageParameter)
        public IHttpActionResult GetItemPage(int PageNumber=0, int PageSize = 0, string itemCode = "", string cardCode = "", string U_ItemCodeEC = "")
        {
            DataTable dt = new DataTable();
            Item oItm = new Item();
            int iSkip = 0;
            dynamic json = null;
            //  iSkip = (pageParameter.PageNumber - 1) * pageParameter.PageSize;
            iSkip = (PageNumber - 1) * PageSize;
            using (SqlConnection connection = new SqlConnection(strConection))
            {
                String sql = string.Format("Sp_AYB_WebAPI {0}, '{1}', '{2}', '{3}', '{4}', '{5}'", 1, itemCode, iSkip, PageSize, cardCode, U_ItemCodeEC);
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
            if (RowCount > 0)
            {
                ITEMS += "[";
                for (int j = 0; j < RowCount; j++)
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

                json = JsonConvert.DeserializeObject(ITEMS);
                return Ok(json);
            }
            else
            {
                //json  = "Sin articulos";

                //  json = JsonConvert.SerializeObject(oItm);
                ITEMS = "{}";
                json = JsonConvert.DeserializeObject(ITEMS);
                return Ok(json);
            }
            //return Ok(json);
        }

    }
}