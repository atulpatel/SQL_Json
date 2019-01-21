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
    public class CustomerController : ControllerBase
    {
        private readonly ICreateCustomer _createCustomer;
        private readonly ICreateCustomerDynamic _createCustomerDynamic;
        private readonly IGetCustomers _getCustomers;

        public CustomerController(ICreateCustomer createCustomer,
            ICreateCustomerDynamic createCustomerDynamic,
            IGetCustomers getCustomers)
        {
            _createCustomer = createCustomer;
            _createCustomerDynamic = createCustomerDynamic;
            _getCustomers = getCustomers;
        }
        // GET: api/Customer
        [HttpGet]
        public async Task<List<Customer>> Get(int id)
        {
            return await _getCustomers.Handler(new Query() {Id =id });
        }
        
        // POST: api/Customer
        [HttpPost]
        public async Task<int> Post(Customer customer)
        {
            return  await _createCustomer.Handler(customer);
        }

        [HttpPost]
        [Route("PostDynamic")]
        public async Task<int> PostDynamic(CreateCustomerCommand customer)
        {
            return await _createCustomerDynamic.Handler(customer);
        }

        // PUT: api/Customer/5
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
