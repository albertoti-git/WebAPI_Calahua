using System.Threading.Tasks;
using System.Web.Http;
using WebApiRESTv1.Repositories;


namespace WebApiRESTv1.Controllers
{
    [Authorize]
    [RoutePrefix("api/prices")]
    public class PriceController : ApiController
    {
        private readonly IPriceRepository _repository;

        public PriceController(IPriceRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("GetPrices")]
        public async Task<IHttpActionResult> GetPrices([FromUri] string cardCode = null, string itemCode = null)
        {
            if (string.IsNullOrWhiteSpace(cardCode))
            {
                return BadRequest("Debe proporcionar el parámetro cardCode.");
            }
            var result = await _repository.GetPricesAsync(cardCode, itemCode);
            return Ok(result);
        }
    }
}