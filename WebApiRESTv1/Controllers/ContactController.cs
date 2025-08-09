using System.Threading.Tasks;
using System.Web.Http;
using WebApiRESTv1.Repositories;


namespace WebApiRESTv1.Controllers
{
    [Authorize]
    [RoutePrefix("api/contact")]
    public class ContactController : ApiController
    {
        private readonly IContactRepository _repository;

        public ContactController(IContactRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("GetContacts")]
        public async Task<IHttpActionResult> GetContacts([FromUri] string cardCode = null)
        {
            var result = await _repository.GetContactsAsync(cardCode);
            return Ok(result);
        }
    }
}