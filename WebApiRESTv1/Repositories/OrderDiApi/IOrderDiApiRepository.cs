using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApiRESTv1.DTO;
using WebApiRESTv1.Models;
using WebApiRESTv1.Util;

namespace WebApiRESTv1.Repositories
{
    public interface IOrderDiApiRepository
    {
        Response<List<object>> GuardarPedidos(List<PedidoRequest> pedidos, string usuario, BoObjectTypes documentCode);

    }
}