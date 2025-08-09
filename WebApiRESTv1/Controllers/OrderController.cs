using SAPbobsCOM;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using WebApiRESTv1.Models;
using WebApiRESTv1.Repositories;
using WebApiRESTv1.Util;

namespace WebApiRESTv1.Controllers
{
    [Authorize]
    [RoutePrefix("api/orders")]
    public class OrderController : ApiController
    {
        private readonly IOrderDiApiRepository _orderDiApiRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderDiApiRepository orderDiApiRepository, IOrderRepository orderRepository)
        {
            _orderDiApiRepository = orderDiApiRepository;
            _orderRepository = orderRepository;
        }

        [Route("save")]
        public IHttpActionResult Save([FromBody] List<PedidoRequest> pedidos)
        {
            var username = User.Identity?.Name ?? "Desconocido";
            var response = _orderDiApiRepository.GuardarPedidos(pedidos, username, BoObjectTypes.oOrders);
            return Ok(response);
        }

        [HttpGet]
        [Route("get")]
        public async Task<IHttpActionResult> GetOrders([FromUri] string docEntry = null, string cardCode = null)
        {
            if (string.IsNullOrWhiteSpace(docEntry) && string.IsNullOrWhiteSpace(cardCode))
            {
                return BadRequest("Debe proporcionar al menos un parámetro: docEntry o cardCode.");
            }
            var orders = await _orderRepository.GetOrdersAsync(docEntry, cardCode);
            return Ok(orders);
        }
    }

}
