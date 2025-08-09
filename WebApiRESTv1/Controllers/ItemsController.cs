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

namespace WebApiRESTv1.Controllers
{
    [Authorize]
    public class ItemsController : ApiController
    {
        string strConection = ConfigurationManager.AppSettings.Get("bdcon");
        [Route("GetItemsPage")]
        //  public IHttpActionResult GetItemPage(PageParameter pageParameter)
        public IHttpActionResult GetItemPage(int PageNumber, int PageSize)
        {
            DataTable dt = new DataTable();
            Item oItm = new Item();
            int iSkip = 0;
            dynamic json = null;
            //  iSkip = (pageParameter.PageNumber - 1) * pageParameter.PageSize;
            iSkip = (PageNumber - 1) * PageSize;
            using (SqlConnection connection = new SqlConnection(strConection))
            {
                String sql = string.Format("Sp_AYB_WebAPI {0}, '{1}', '{2}', '{3}'", 1, "", iSkip, PageSize);
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

        /// <summary>
        /// Obtiene los Items
        /// </summary>
        /// <returns>colecion de items</returns>
        /// 
        [Route("Items")]
        public IHttpActionResult GetItem(string ItemCode = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(strConection))
            {
                String sql = string.Format("Sp_AYB_WebAPI {0}, '{1}', '{2}', '{3}'", 1, ItemCode, 1, 1 );
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
            for (int j = 0; j < RowCount ; j++)
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

        //[Route("ItemCode")]
        //public IHttpActionResult GetItemCode(int Parameter)
        //{
        //    DataTable dt = new DataTable();
        //    using (SqlConnection connection = new SqlConnection(strConection))
        //    {
        //        String sql = string.Format("Sp_AYB_WebAPI {0}, '{1}'", 1, Parameter);
        //        SqlCommand cmd = new SqlCommand();
        //        SqlDataAdapter sqlDA;
        //        connection.Open();
        //        cmd.CommandText = sql;
        //        cmd.CommandType = CommandType.Text;
        //        cmd.Connection = connection;
        //        sqlDA = new SqlDataAdapter(cmd);
        //        sqlDA.Fill(dt);

        //    }
        //    int ColCount = dt.Columns.Count;
        //    int RowCount = dt.Rows.Count;
        //    string ITEMS = string.Empty;
        //    ITEMS += "[";
        //    for (int j = 0; j < RowCount - 1; j++)
        //    {
        //        string ITEM = string.Empty;
        //        ITEM = "{";
        //        string ITEMVALUE = string.Empty;
        //        for (int i = 0; i < ColCount - 1; i++)
        //        {
        //            string ColName = dt.Columns[i].ColumnName;
        //            ITEMVALUE = dt.Rows[j][i].ToString();
        //            Type tipoDato = dt.Rows[j][i].GetType();
        //            if (tipoDato.Name == "String")
        //            {
        //                ITEM += "\"" + ColName + "\"" + " : " + "\"" + ITEMVALUE + "\"";
        //            }
        //            else
        //            {
        //                ITEM += "\"" + ColName + "\"" + " : " + ITEMVALUE;
        //            }

        //            ITEM += ",";
        //        }
        //        if (ITEMVALUE.Length > 0)
        //        {
        //            ITEM = ITEM.Substring(0, ITEM.Length - 1);
        //        }
        //        ITEM += "},";
        //        ITEMS += ITEM;
        //    }
        //    ITEMS = ITEMS.Substring(0, ITEMS.Length - 1);
        //    ITEMS += "]";

        //    dynamic json = JsonConvert.DeserializeObject(ITEMS);
        //    return Ok(json);
        //}

        //[Route("CodeBar")]
        //public IHttpActionResult GetItemCodeBar(int Parameter)
        //{
        //    DataTable dt = new DataTable();
        //    using (SqlConnection connection = new SqlConnection(strConection))
        //    {
        //        String sql = string.Format("Sp_AYB_WebAPI {0}, '{1}'", 1, Parameter);
        //        SqlCommand cmd = new SqlCommand();
        //        SqlDataAdapter sqlDA;
        //        connection.Open();
        //        cmd.CommandText = sql;
        //        cmd.CommandType = CommandType.Text;
        //        cmd.Connection = connection;
        //        sqlDA = new SqlDataAdapter(cmd);
        //        sqlDA.Fill(dt);

        //    }
        //    int ColCount = dt.Columns.Count;
        //    int RowCount = dt.Rows.Count;
        //    string ITEMS = string.Empty;
        //    ITEMS += "[";
        //    for (int j = 0; j < RowCount - 1; j++)
        //    {
        //        string ITEM = string.Empty;
        //        ITEM = "{";
        //        string ITEMVALUE = string.Empty;
        //        for (int i = 0; i < ColCount - 1; i++)
        //        {
        //            string ColName = dt.Columns[i].ColumnName;
        //            ITEMVALUE = dt.Rows[j][i].ToString();
        //            Type tipoDato = dt.Rows[j][i].GetType();
        //            if (tipoDato.Name == "String")
        //            {
        //                ITEM += "\"" + ColName + "\"" + " : " + "\"" + ITEMVALUE + "\"";
        //            }
        //            else
        //            {
        //                ITEM += "\"" + ColName + "\"" + " : " + ITEMVALUE;
        //            }

        //            ITEM += ",";
        //        }
        //        if (ITEMVALUE.Length > 0)
        //        {
        //            ITEM = ITEM.Substring(0, ITEM.Length - 1);
        //        }
        //        ITEM += "},";
        //        ITEMS += ITEM;
        //    }
        //    ITEMS = ITEMS.Substring(0, ITEMS.Length - 1);
        //    ITEMS += "]";

        //    dynamic json = JsonConvert.DeserializeObject(ITEMS);
        //    return Ok(json);
        //}

        //// GET: api/Items
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Items/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Items
        //public void Post([FromBody] Quotation oQuotation)
        //{
        //    SAPbobsCOM.Company oComp = null;
        //    SAPbobsCOM.Documents odoc = null;

        //    //odoc .car
        //}

        //// PUT: api/Items/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Items/5
        //public void Delete(int id)
        //{
        //}
    }
}
