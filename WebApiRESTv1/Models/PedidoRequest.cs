using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiRESTv1.Models
{
    public class PedidoRequest
    {
        public Pedido Pedido { get; set; }
        public List<DetallePedido> DetallePedido { get; set; }
    }
    public class Pedido
    {
        public string Cliente { get; set; }           // CardCode         nvarchar(15)
        public string Nombre { get; set; }            // CardName         nvarchar(100)
        public string OcReferencia { get; set; }      // NumAtCard        nvarchar(100)

        public short FormaPago { get; set; }          // GroupNum         smallint
        public string Moneda { get; set; }            // DocCur           nvarchar(3)
        public DateTime Fecha { get; set; }           // DocDate          datetime
        public DateTime FechaEntrega { get; set; }    // DocDueDate       datetime

        public string Destino { get; set; }           // ShipToCode       nvarchar(50)
        public int EmpleadoVentas { get; set; }       // SlpCode          int
        public string Comentarios { get; set; }       // Comments         nvarchar(254)
        public int Series { get; set; }            // Series           nvarchar(254)
        public int CodigoContacto { get; set; }            // Series           nvarchar(254)
        public double TipoCambio { get; set; }            // Series           nvarchar(254)

    }


    public class DetallePedido
    {
        public string NroArticuloSku { get; set; }        // ItemCode         nvarchar(50)
        public int NroLinea { get; set; }                 // LineNum          int(10)
        public string Descripcion { get; set; }           // Dscription       nvarchar(200)
        public string CodigoBarras { get; set; }          // CodeBars         nvarchar(254)
        public decimal CantidadCajas { get; set; }        // Quantity         numeric(19,6)
        public decimal Precio { get; set; }               // Price            numeric(19,6)
        public decimal Descuento { get; set; }            // DiscPrcnt        numeric(19,6)
        public decimal LineaTotal { get; set; }           // LineTotal        numeric(19,6)
        public string IndicadorImpuestos { get; set; }    // TaxCode          nvarchar(8)
        public string Almacen { get; set; }               // WhsCode          nvarchar(8)
    }

}