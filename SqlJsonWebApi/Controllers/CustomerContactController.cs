using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlJsonWebApi.Models;
using SqlJsonWebApi.Services.Customer;

namespace SqlJsonWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerContactController : ControllerBase
    {
        private readonly IGetCustomerContactsDynamic _getCustomerContactsDynamic;
        private readonly IGetCustomerContacts _getCustomerContacts;
        private readonly ICreateCustomerContact _createCustomerContact;

        public CustomerContactController(IGetCustomerContactsDynamic getCustomerContactsDynamic
            ,IGetCustomerContacts getCustomerContacts
            ,ICreateCustomerContact createCustomerContact)
        {
            _getCustomerContactsDynamic = getCustomerContactsDynamic;
            _getCustomerContacts = getCustomerContacts;
            _createCustomerContact = createCustomerContact;
        }
        // GET: api/CustomerContact
        [HttpGet]
        [Route("GetDynamic")]
        public async Task<dynamic> GetDynamic(int customerId)
        {
            return await _getCustomerContactsDynamic.Handler(new GetCustomerContactQuery() { CustomerId = customerId });
        }

        [HttpGet]
        public async Task<List<CustomerContact>> Get(int customerId)
        {
            return await _getCustomerContacts.Handler(new GetCustomerContactQuery() { CustomerId = customerId });
        }

        //// GET: api/CustomerContact/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/CustomerContact
        [HttpPost]
        public async Task<int> Post(CreateCustomerContactCommand command)
        {
            return await _createCustomerContact.Handler(command);
        }

        // PUT: api/CustomerContact/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
