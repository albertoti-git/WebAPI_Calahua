using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApiRESTv1.DTO;
using WebApiRESTv1.Util;

namespace WebApiRESTv1.Repositories.Quotation
{

    public class QuoteRepository : IQuoteRepository
    {
        private readonly string _connectionString;

        public QuoteRepository()
        {
            _connectionString = ConfigurationManager.AppSettings["bdcon"];
        }

        public async Task<Response<List<QuoteDto>>> GetQuotationsAsync(string docEntry, string cardCode)
        {
            var response = new Response<List<QuoteDto>>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("Sp_AYB_WebAPI", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Opc", SqlDbType.Int).Value = 10;
                    command.Parameters.Add("@StrVale1", SqlDbType.NVarChar).Value = string.Empty;
                    command.Parameters.Add("@StrVale2", SqlDbType.NVarChar).Value = docEntry ?? string.Empty;
                    command.Parameters.Add("@StrVale3", SqlDbType.NVarChar).Value = cardCode ?? string.Empty;

                    await connection.OpenAsync();

                    var quotes = new List<QuoteDto>();
                    var details = new List<QuoteDetailDto>();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            quotes.Add(new QuoteDto
                            {
                                DocEntry = reader.GetInt32(reader.GetOrdinal("DocEntry")),
                                DocDate = reader.IsDBNull(reader.GetOrdinal("DocDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DocDate")),
                                DocStatus = reader["DocStatus"]?.ToString(),
                                CardCode = reader["CardCode"]?.ToString(),
                                CardName = reader["CardName"]?.ToString(),
                                DocDueDate = reader.IsDBNull(reader.GetOrdinal("DocDueDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("DocDueDate")),
                                DocCur = reader["DocCur"]?.ToString(),
                                DocRate = reader.IsDBNull(reader.GetOrdinal("DocRate")) ? 0 : reader.GetDecimal(reader.GetOrdinal("DocRate")),
                                NumAtCard = reader["NumAtCard"]?.ToString(),
                                CntctCode = reader.IsDBNull(reader.GetOrdinal("CntctCode")) ? 0 : reader.GetInt32(reader.GetOrdinal("CntctCode")),
                                Comments = reader["Comments"]?.ToString(),
                                GroupNum = reader.IsDBNull(reader.GetOrdinal("GroupNum")) ? (short)0 : reader.GetInt16(reader.GetOrdinal("GroupNum")),
                                ShipToCode = reader["ShipToCode"]?.ToString(),
                                U_B1SYS_MainUsage = reader["U_B1SYS_MainUsage"]?.ToString(),
                                DocTotal = reader.IsDBNull(reader.GetOrdinal("DocTotal"))
                                            ? 0m
                                            : reader.GetDecimal(reader.GetOrdinal("DocTotal")),
                                VatSum = reader.IsDBNull(reader.GetOrdinal("VatSum"))
                                            ? 0m
                                            : reader.GetDecimal(reader.GetOrdinal("VatSum")),
                                PaidToDate = reader.IsDBNull(reader.GetOrdinal("PaidToDate"))
                                            ? 0m
                                            : reader.GetDecimal(reader.GetOrdinal("PaidToDate")),
                                QuoteDetails = new List<QuoteDetailDto>()
                            });
                        }

                        if (await reader.NextResultAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                details.Add(new QuoteDetailDto
                                {
                                    DocEntry = reader.IsDBNull(reader.GetOrdinal("DocEntry")) ? 0 : reader.GetInt32(reader.GetOrdinal("DocEntry")),
                                    LineStatus = reader["LineStatus"]?.ToString() ?? "",
                                    LineNum = reader.IsDBNull(reader.GetOrdinal("LineNum")) ? 0 : reader.GetInt32(reader.GetOrdinal("LineNum")),
                                    ItemCode = reader["ItemCode"]?.ToString() ?? "",
                                    Dscription = reader["Dscription"]?.ToString() ?? "",
                                    Quantity = reader.IsDBNull(reader.GetOrdinal("Quantity")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Quantity")),
                                    OpenQty = reader.IsDBNull(reader.GetOrdinal("OpenQty")) ? 0 : reader.GetDecimal(reader.GetOrdinal("OpenQty")),
                                    Price = reader.IsDBNull(reader.GetOrdinal("Price")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Price")),
                                    PriceBefDi = reader.IsDBNull(reader.GetOrdinal("PriceBefDi")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PriceBefDi")),
                                    LineTotal = reader.IsDBNull(reader.GetOrdinal("LineTotal")) ? 0 : reader.GetDecimal(reader.GetOrdinal("LineTotal")),
                                    VatSum = reader.IsDBNull(reader.GetOrdinal("VatSum")) ? 0 : reader.GetDecimal(reader.GetOrdinal("VatSum")),
                                    VatPrcnt = reader.IsDBNull(reader.GetOrdinal("VatPrcnt")) ? 0 : reader.GetDecimal(reader.GetOrdinal("VatPrcnt")),
                                    OpenSum = reader.IsDBNull(reader.GetOrdinal("OpenSum")) ? 0 : reader.GetDecimal(reader.GetOrdinal("OpenSum"))
                                });

                            }
                        }
                    }

                    // Mapear detalles a la orden correspondiente
                    foreach (var quote in quotes)
                    {
                        quote.QuoteDetails = details.Where(d => d.DocEntry == quote.DocEntry).ToList();
                    }
                    response.Mensaje = "Operación Completada, ejecución exitosa";
                    response.IsSuccess = true;
                    response.Dato = quotes;
                }
            }
            catch (Exception ex)
            {
                response.Mensaje = "Error al obtener las ofertas";
                response.IsSuccess = false;
                response.MensajeDev = ex.Message;
            }

            return response;
        }

    }
}