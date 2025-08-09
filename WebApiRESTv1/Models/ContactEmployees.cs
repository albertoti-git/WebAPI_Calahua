using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiRESTv1.Models
{
    public class ContactEmployees
    {
        public string Position { get; set; }
        public string Address { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string MobilePhone { get; set; }
        public string Fax { get; set; }
        public string E_Mail { get; set; }
        public string Pager { get; set; }
        public string Remarks1 { get; set; }
        public string Remarks2 { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public UserFields UserFields { get; set; }
        public int InternalCode { get; set; }
        public string PlaceOfBirth { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set; }
        public string Profession { get; set; }
        public object CardCode { get; set; }
        public string Title { get; set; }
        public string CityOfBirth { get; set; }
        public bool Active { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }
}