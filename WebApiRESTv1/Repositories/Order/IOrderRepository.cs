using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiRESTv1.DTO;
using WebApiRESTv1.Models;
using WebApiRESTv1.Util;

namespace WebApiRESTv1.Repositories
{
    public interface IOrderRepository
    {
        Task<Response<List<OrderDto>>> GetOrdersAsync(string docEntry, string cardCode);


    }
}
