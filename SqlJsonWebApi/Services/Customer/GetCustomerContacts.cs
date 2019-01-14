using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SqlJsonWebApi.Services.Customer
{
    public class GetCustomerContactQuery {
        public int CustomerId { get; set; }
    }
    public interface IGetCustomerContactsDynamic {
        Task<dynamic> Handler(GetCustomerContactQuery query);
    }
    public class GetCustomerContactsDynamic : IGetCustomerContactsDynamic
    {
        private readonly IDbConnection _dbConnection;

        public GetCustomerContactsDynamic(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<dynamic> Handler(GetCustomerContactQuery query)
        {
            var sql = @"SELECT js.value as CustomerContacts
                        FROM Customer c
                        CROSS APPLY OPENJSON(c.[CustomerContacts], '$') AS js
                        WHERE c.Id = @CustomerId
                        ";
            var result =await _dbConnection.QueryAsync<dynamic>(sql, query);

            foreach (var item in result)
            {
                item.CustomerContacts = JsonConvert.DeserializeObject<dynamic>(item.CustomerContacts);
            }
            return result;
        }
    }

    public interface IGetCustomerContacts
    {
        Task<List<Models.CustomerContact>> Handler(GetCustomerContactQuery query);
    }
    public class GetCustomerContacts : IGetCustomerContacts
    {
        private readonly IDbConnection _dbConnection;

        public GetCustomerContacts(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<List<Models.CustomerContact>> Handler(GetCustomerContactQuery query)
        {
            var sql = @"SELECT js.*
                        FROM Customer c
                        CROSS APPLY OPENJSON(c.[CustomerContacts], '$') WITH (
		                        Id uniqueidentifier
		                        ,Title VARCHAR(10)
		                        ,FirstName VARCHAR(50)
		                        ,MiddleName VARCHAR(50)
		                        ,LastName VARCHAR(50)
		                        ,CompanyName VARCHAR(50)
		                        ,SalesPerson VARCHAR(100)
		                        ,EmailAddress VARCHAR(100)
		                        ,Phone VARCHAR(20)
		                        ) AS js
                        WHERE c.id = @CustomerId";
            var result = await _dbConnection.QueryAsync<Models.CustomerContact>(sql, query);
            return result.ToList();
        }
    }
}
