using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiRESTv1.DTO;
using WebApiRESTv1.Util;

namespace WebApiRESTv1.Repositories
{
    public interface IQuoteRepository
    {
        Task<Response<List<QuoteDto>>> GetQuotationsAsync(string docEntry, string cardCode);

    }
}
