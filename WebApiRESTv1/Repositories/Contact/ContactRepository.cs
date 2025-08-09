using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebApiRESTv1.DTO;
using WebApiRESTv1.Util;

namespace WebApiRESTv1.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly string _connectionString;

        public ContactRepository()
        {
            _connectionString = ConfigurationManager.AppSettings["bdcon"];
        }

        public async Task<Response<List<ContactDto>>> GetContactsAsync(string cardCode)
        {
            var response = new Response<List<ContactDto>>();
            var contacts = new List<ContactDto>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("Sp_AYB_WebAPI", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Opc", SqlDbType.Int).Value = 7; // Opción para contactos
                    command.Parameters.Add("@StrVale1", SqlDbType.NVarChar).Value = string.Empty;
                    command.Parameters.Add("@StrVale2", SqlDbType.NVarChar).Value = cardCode ?? "";
                    command.Parameters.Add("@StrVale3", SqlDbType.NVarChar).Value = string.Empty;

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var contact = new ContactDto
                            {
                                CardCode = reader["CardCode"].ToString(),
                                CntctCode = reader["CntctCode"].ToString(),
                                Name = reader["Name"].ToString(),
                                Position = reader["Position"].ToString(),
                                Email = reader["E_MailL"].ToString()
                            };

                            contacts.Add(contact);
                        }
                    }
                }

                return response.Ok(contacts);
            }
            catch (Exception ex)
            {
                return response.Falla($"Error al obtener contactos: {ex}");
            }
        }
    }
}
