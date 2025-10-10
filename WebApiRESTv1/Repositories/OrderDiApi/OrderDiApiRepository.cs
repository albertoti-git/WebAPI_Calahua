
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApiRESTv1.Models;
using WebApiRESTv1.Util;

namespace WebApiRESTv1.Repositories
{
    public class OrderDiApiRepository : IOrderDiApiRepository
    {
        private readonly Company _company;

        public OrderDiApiRepository(Company company)
        {
            _company = company;
        }

        public Response<List<object>> GuardarPedidos(List<PedidoRequest> pedidos, string usuario, BoObjectTypes documentCode)
        {
            var response = new Response<List<object>>();
            var resultados = new List<object>();
            bool tieneErrores = false;

            foreach (var pedidoRequest in pedidos)
            {
                var pedido = pedidoRequest.Pedido;
                var lineas = pedidoRequest.DetallePedido;

                try
                {
                    Documents oOrder = (Documents)_company.GetBusinessObject(documentCode);

                    // Cabecera
                    oOrder.CardCode = pedido.Cliente;
                    oOrder.CardName = pedido.Nombre;
                    oOrder.DocDate = pedido.Fecha;
                    oOrder.DocDueDate = pedido.FechaEntrega;
                    oOrder.NumAtCard = pedido.OcReferencia;
                    oOrder.Comments = pedido.Comentarios;
                    oOrder.DocCurrency = pedido.Moneda;
                  //  oOrder.GroupNumber = pedido.FormaPago;
                   // oOrder.ShipToCode = pedido.Destino;
                    oOrder.Series = pedido.Series;
                    oOrder.ContactPersonCode = pedido.CodigoContacto;
                    oOrder.DocRate = pedido.TipoCambio;
                    try
                    {
                        oOrder.FederalTaxID = pedido.RFC;
                        oOrder.UserFields.Fields.Item("U_U_DHL_ShipTo").Value = pedido.ShipDHL;
                        oOrder.UserFields.Fields.Item("U_B1SYS_MainUsage").Value = pedido.UsoCFDI;
                        oOrder.UserFields.Fields.Item("U_U_MPAGO").Value = pedido.MetodoPago;
                    }
                    catch (Exception ex)
                    {
                        // Si hay un error al asignar estos campos, se captura la excepción pero no se detiene el proceso.
                        // Esto puede ocurrir si los campos definidos en SAP no existen o si hay un problema con los datos.
                      throw new Exception ($"Error al asignar campos adicionales: {ex.Message}");
                    }


                    if (pedido.UserFields != null && pedido.UserFields.Length > 0)
                    {
                        foreach (var diccionario in pedido.UserFields) // Recorres cada Dictionary<string,string>
                        {
                            if (diccionario != null)
                            {
                                foreach (var kvp in diccionario) // kvp.Key y kvp.Value
                                {
                                    oOrder.UserFields.Fields.Item(kvp.Key).Value = kvp.Value;
                                }
                            }
                        }
                    }

                    try
                    { 
                    if (pedido.Addresses != null && pedido.Addresses.Count > 0)
                    {
                        for (int iIndex3 = 0; iIndex3 < pedido.Addresses.Count; iIndex3++)
                        {
                            if (iIndex3 > 0)
                            {

                                BPAddress oDir = pedido.Addresses[iIndex3];

                                if (oDir.AddressType == "bo_ShipTo")
                                {
                                    if (oDir.AddressName != "")
                                    {
                                        oOrder.ShipToCode = oDir.AddressName;
                                    }
                                    else
                                    {
                                        oOrder.AddressExtension.ShipToStreet = oDir.Street;
                                        oOrder.AddressExtension.ShipToBlock = oDir.Block;
                                        oOrder.AddressExtension.ShipToZipCode = oDir.ZipCode;
                                        oOrder.AddressExtension.ShipToCity = oDir.City;
                                        oOrder.AddressExtension.ShipToCounty = oDir.County;
                                        oOrder.AddressExtension.ShipToCountry = oDir.Country;
                                        oOrder.AddressExtension.ShipToState = oDir.State;                                    
                                        oOrder.AddressExtension.ShipToBuilding= oDir.BuildingFloorRoom;
                                        oOrder.AddressExtension.ShipToStreetNo = oDir.StreetNo;
                                        oOrder.AddressExtension.ShipToGlobalLocationNumber = oDir.GlobalLocationNumber;
                                        
                                    }

                                } else
                                    {
                                    if (oDir.AddressName != "")
                                    {
                                        oOrder.PayToCode = oDir.AddressName;
                                    }
                                    else
                                    {
                                        oOrder.AddressExtension.BillToStreet = oDir.Street;
                                        oOrder.AddressExtension.BillToBlock = oDir.Block;
                                        oOrder.AddressExtension.BillToZipCode = oDir.ZipCode;
                                        oOrder.AddressExtension.BillToCity = oDir.City;
                                        oOrder.AddressExtension.BillToCounty = oDir.County;
                                        oOrder.AddressExtension.BillToCountry = oDir.Country;
                                        oOrder.AddressExtension.BillToState = oDir.State;
                                        oOrder.AddressExtension.BillToBuilding = oDir.BuildingFloorRoom;
                                        oOrder.AddressExtension.BillToStreetNo = oDir.StreetNo;
                                        oOrder.AddressExtension.BillToGlobalLocationNumber = oDir.GlobalLocationNumber;
                                    }
                                }                                   
                             
                            }
                                                     

                        }
                    }

                       }
                    catch (Exception ex)
                    {                         // Si hay un error al asignar estos campos, se captura la excepción pero no se detiene el proceso.
                        // Esto puede ocurrir si los campos definidos en SAP no existen o si hay un problema con los datos.
                        throw new Exception($"Error al asignar direcciones: {ex.Message}");
                    }
                    // Detalle
                    foreach (var linea in lineas)
                    {
                        oOrder.Lines.ItemCode = linea.NroArticuloSku;
                        oOrder.Lines.ItemDescription = linea.Descripcion;
                        oOrder.Lines.Quantity = (double)linea.CantidadCajas;
                        oOrder.Lines.UnitPrice = (double)linea.Precio;
                        oOrder.Lines.DiscountPercent = (double)linea.Descuento;
                        oOrder.Lines.TaxCode = linea.IndicadorImpuestos;
                        oOrder.Lines.WarehouseCode = linea.Almacen;
                        oOrder.Lines.Add();
                    }

                    // Guardar documento
                    int res = oOrder.Add();

                    if (res != 0)
                    {
                        _company.GetLastError(out int errCode, out string errMsg);
                        resultados.Add(new
                        {
                            pedido = pedido.OcReferencia,
                            estado = "Error",
                            mensaje = $"[{errCode}] {errMsg}"
                        });
                        tieneErrores = true;
                    }
                    else
                    {
                        string docEntry = _company.GetNewObjectKey();
                        resultados.Add(new
                        {
                            pedido = pedido.OcReferencia,
                            estado = "OK",
                            docEntry = docEntry
                        });
                    }
                }
                catch (Exception ex)
                {
                    resultados.Add(new
                    {
                        pedido = pedido.OcReferencia,
                        estado = "Excepción",
                        mensaje = ex.Message
                    });
                    tieneErrores = true;
                }
            }

            if (tieneErrores)
            {
                return response.Falla("Uno o más pedidos no pudieron ser registrados correctamente.").Ok(resultados);
            }
            else
            {
                return response.Ok(resultados);
            }

        }


    }
}


