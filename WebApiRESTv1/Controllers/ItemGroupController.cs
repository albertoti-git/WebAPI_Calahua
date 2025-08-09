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
    public class ItemGroupController : ApiController
    {
        string strConection = ConfigurationManager.AppSettings.Get("bdcon");
        public IHttpActionResult Get(int Parameter)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(strConection))
            {
                String sql = string.Format("Sp_AYB_WebAPI {0}, '{1}'", 1, Parameter);
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
    }
}
