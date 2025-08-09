
using SAPbobsCOM;
using System;
using System.Collections.Generic;
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
                    oOrder.GroupNumber = pedido.FormaPago;
                    oOrder.ShipToCode = pedido.Destino;
                    oOrder.Series = pedido.Series;
                    oOrder.ContactPersonCode = pedido.CodigoContacto;
                    oOrder.DocRate = pedido.TipoCambio;

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


