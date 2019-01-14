using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SqlJsonWebApi.Services.Customer
{
    public class CreateCustomerContactCommand
    {
        public int CustomerId { get; set; }
        public Models.CustomerContact CustomerContact { get; set; }
    }
    public class CreateCustomerContact : ICreateCustomerContact
    {
        private readonly IDbConnection _dbConnection;

        public CreateCustomerContact(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<int> Handler(CreateCustomerContactCommand command)
        {
            //TODO: Validate command.
            if (command.CustomerContact == null || command.CustomerId == 0)
            {
                throw new ArgumentNullException("command", "CreateCustomerContactCommand is null");
            }
            command.CustomerContact.Id = Guid.NewGuid();
            var contact = JsonConvert.SerializeObject(command.CustomerContact);

            var sql = @"UPDATE [dbo].[Customer]
                            SET [CustomerContacts] = JSON_MODIFY(CustomerContacts,'append $',JSON_QUERY(@CustomerContact,'$'))
                        WHERE Id=@CustomerId";

            return await _dbConnection.ExecuteAsync(sql, new  { command.CustomerId, CustomerContact = contact });
        }
    }

    public interface ICreateCustomerContact
    {
        Task<int> Handler(CreateCustomerContactCommand command);
    }
}
