using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiRESTv1.Models
{
    public class ItemGroup
    {
        public int GroupCode { get; set; }
        public string GroupName { get; set; }
        public UserFields UserFields { get; set; }
    }
}