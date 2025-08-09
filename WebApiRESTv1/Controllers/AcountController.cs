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

namespace WebApiRESTv1.Controllers
{
    [AllowAnonymous]
    public class AcountController : ApiController
    {
        /// <summary>
        /// Metodo encargado de realizar la autenticacion y generar tokens
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <returns></returns>
        /// 
     
        [HttpPost]
        public IHttpActionResult Login(Login loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string strConection = ConfigurationManager.AppSettings.Get("bdcon");
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(strConection))
            {
                String sql = string.Format("Sp_AYB_WebAPI {0}, '{1}', '{2}' ", 5, loginDTO.UserName, loginDTO.PassWord);
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter sqlDA;
                connection.Open();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = connection;
                sqlDA = new SqlDataAdapter(cmd);
                sqlDA.Fill(dt);

            }
            string psw = dt.Rows[0][1].ToString();
            bool isCredentialValid = (loginDTO.PassWord == psw);

            if (isCredentialValid)
            {
                var token = TokenGenerator.GenerateTokenJwt(loginDTO.UserName);
                return Ok(token);
            }
            else
            {
                return Unauthorized();//status code 401
            }
        }
    }
}
