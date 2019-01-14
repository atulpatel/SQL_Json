using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Models=SqlJsonWebApi.Models;

namespace SqlJsonWebApi.Services.Customer
{
    public class CustomerInsertModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public string CustomerContacts { get; set; }
    }
    public interface ICreateCustomer
    {
        Task<int> Handler(Models.Customer customer);
    }
    public class CreateCustomer : ICreateCustomer
    {
        private readonly IDbConnection _dbConnection;

        public CreateCustomer(IDbConnection  dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<int> Handler(Models.Customer customer)
        {
            //You can AutoMapper for this
            var customerInsertModel = new CustomerInsertModel()
            {
                Name = customer.Name,
                Active = customer.Active,
                CustomerContacts = JsonConvert.SerializeObject(customer.CustomerContacts??new List<Models.CustomerContact>())
            };

            string sql = @"INSERT INTO [dbo].[Customer]
                                   ([Name]
                                   ,[Active]
                                   ,[CustomerContacts])
                                    OUTPUT INSERTED.Id
                             VALUES
                                   (@Name
                                   ,@Active
                                   ,@CustomerContacts)";

            using (var connection = _dbConnection)
            {
                connection.Open();
                return await connection.QuerySingleAsync<int>(sql, customerInsertModel);
            }

        }
    }
}
