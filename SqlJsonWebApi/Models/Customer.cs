using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SqlJsonWebApi.Models
{
    //public class Customer
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public bool Active { get; set; }

    //    public dynamic CustomerContacts  { get; set; }
    //}


    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }

        public List<CustomerContact> CustomerContacts { get; set; }
    }
}
