using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using WebApiRESTv1.DTO;
using WebApiRESTv1.Util;

namespace WebApiRESTv1.Repositories.Price
{
    public class PriceRepository : IPriceRepository
    {
        private readonly string _connectionString;

        public PriceRepository()
        {
            _connectionString = ConfigurationManager.AppSettings["bdcon"];
        }

        public async Task<Response<List<PriceDto>>> GetPricesAsync(string cardCode, string itemCode)
        {
            var response = new Response<List<PriceDto>>();
            var prices = new List<PriceDto>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("Sp_AYB_WebAPI", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Opc", SqlDbType.Int).Value = 8;
                    command.Parameters.Add("@StrVale1", SqlDbType.NVarChar).Value = string.Empty;
                    command.Parameters.Add("@StrVale2", SqlDbType.NVarChar).Value = cardCode ?? string.Empty;
                    command.Parameters.Add("@StrVale3", SqlDbType.NVarChar).Value = itemCode ?? string.Empty;

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var contact = new PriceDto
                            {
                                CardCode = reader["CardCode"].ToString(),
                                CardName = reader["CardName"].ToString(),
                                ListNum = reader["ListNum"].ToString(),
                                ItemCode = reader["ItemCode"].ToString(),
                                Price = reader["Price"].ToString()
                            };

                            prices.Add(contact);
                        }
                    }
                }

                return response.Ok(prices);
            }
            catch (Exception ex)
            {
                return response.Falla($"Error al obtener precios: {ex}");
            }
        }
    }
}