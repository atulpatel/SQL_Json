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
    public class CreateCustomerCommand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }

        public dynamic CustomerContacts { get; set; }
    }
    public interface ICreateCustomerDynamic
    {
        Task<int> Handler(CreateCustomerCommand customer);
    }
    public class CreateCustomerDynamic : ICreateCustomerDynamic
    {
        private readonly IDbConnection _dbConnection;

        public CreateCustomerDynamic(IDbConnection  dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<int> Handler(CreateCustomerCommand customer)
        {
            //You can AutoMapper for this
            var customerInsertModel = new
            {
                Name = customer.Name,
                Active = customer.Active,
                CustomerContacts = JsonConvert.SerializeObject(customer.CustomerContacts)
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
