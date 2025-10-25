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
using Microsoft.AspNetCore.JsonPatch;
using System.Dynamic;
using Newtonsoft.Json.Converters;
using SAPbobsCOM;


namespace WebApiRESTv1.Controllers
{
    [Authorize]
    public class PaymentsController : ApiController
    {

        [HttpPost]
        [Route("api/IncomingPayments")]
        public HttpResponseMessage Post_IncomingPayments([FromBody] IncPayments pay)
        {
            try
            {
                ConexionSAP conexionSAP = ConexionSAP.GetInstance;
                Payments oIncomingPayment = (Payments)(dynamic)conexionSAP.CompanySBO.GetBusinessObject(BoObjectTypes.oIncomingPayments);
                BusinessPartners oSocios = (BusinessPartners)(dynamic)conexionSAP.CompanySBO.GetBusinessObject(BoObjectTypes.oBusinessPartners);

                // Validar que el socio de negocio exista
                if (!oSocios.GetByKey(pay.CardCode))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new
                    {
                        code = 400,
                        message = "El socio de negocio no existe",
                        cardCode = pay.CardCode
                    });
                }

                // Asignar datos principales
                oIncomingPayment.CardCode = pay.CardCode;
                oIncomingPayment.DocDate = pay.DocDate;
                oIncomingPayment.TransferReference = pay.TransferRef;
                oIncomingPayment.TransferAccount = pay.TransferAccount;
                oIncomingPayment.TransferSum = pay.TransferSum;
                oIncomingPayment.Remarks = pay.Comments;
                oIncomingPayment.JournalRemarks = pay.Memos;    

                // Campos de usuario (si vienen)
                if (pay.UserFields != null && pay.UserFields.Length > 0)
                {
                    foreach (var diccionario in pay.UserFields)
                    {
                        if (diccionario != null)
                        {
                            foreach (var kvp in diccionario)
                            {
                                oIncomingPayment.UserFields.Fields.Item(kvp.Key).Value = kvp.Value;
                            }
                        }
                    }
                }

                // Intentar agregar el documento en SAP
                if (oIncomingPayment.Add() != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new
                    {
                        code = 400,
                        message = conexionSAP.CompanySBO.GetLastErrorDescription(),
                        data = pay
                    });
                }

                // Si se creó correctamente, obtener DocEntry
                pay.DocEntry = Convert.ToInt32(conexionSAP.CompanySBO.GetNewObjectKey());

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    code = 200,
                    message = "Pago recibido correctamente",
                    docEntry = pay.DocEntry
                });
            }
            catch (Exception ex)
            {
                // Cualquier excepción inesperada
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new
                {
                    code = 500,
                    message = ex.Message
                });
            }
        }

    }
}