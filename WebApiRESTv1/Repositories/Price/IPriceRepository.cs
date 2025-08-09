using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiRESTv1.DTO;
using WebApiRESTv1.Util;

namespace WebApiRESTv1.Repositories
{
    public interface IPriceRepository
    {
        Task<Response<List<PriceDto>>> GetPricesAsync(string cardCode, string itemCode);

    }
}
