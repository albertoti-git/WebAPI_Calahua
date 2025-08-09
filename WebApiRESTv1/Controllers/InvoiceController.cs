using System.Threading.Tasks;
using System.Web.Http;
using WebApiRESTv1.Repositories;

namespace WebApiRESTv1.Controllers
{
    [Authorize]
    [RoutePrefix("api/invoice")]
    public class InvoiceController : ApiController
    {
        private readonly IInvoiceRepository _invoiceRepository;
        public InvoiceController(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        [HttpGet]
        [Route("get")]
        public async Task<IHttpActionResult> GetInvoice([FromUri] string docEntry = null, string cardCode = null)
        {
            var orders = await _invoiceRepository.GetInvoiceAsync(docEntry, cardCode);
            return Ok(orders);
        }
    }
}