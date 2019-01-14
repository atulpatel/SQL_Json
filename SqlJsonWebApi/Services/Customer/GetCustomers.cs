using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Models = SqlJsonWebApi.Models;
namespace SqlJsonWebApi.Services.Customer
{
    public class CustomerResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public string CustomerContacts { get; set; }
    }
    public class Query
    {
        public int Id { get; set; }
    }
    public class GetCustomers : IGetCustomers
    {
        private readonly IDbConnection _dbConnection;

        public GetCustomers(IDbConnection  dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<List<Models.Customer>> Handler(Query query)
        {

            string buildSql(string id) =>  $@"SELECT [Id]
                                              ,[Name]
                                              ,[Active]
                                              ,[CustomerContacts]
                                          FROM [dbo].[Customer]
                                          {id}";
            var idsql = query.Id !=0? "WHERE Id=@Id": string.Empty;
            var sql = buildSql(idsql);

            var result = await _dbConnection.QueryAsync<CustomerResponseModel>(sql, query);
            var response = new List<Models.Customer>(); 
            foreach (var item in result)
            {
                response.Add(new Models.Customer() {
                    Id= item.Id,
                    Name= item.Name,
                    Active = item.Active,
                    CustomerContacts = JsonConvert.DeserializeObject<List<Models.CustomerContact>>(item.CustomerContacts??string.Empty)
                    //CustomerContacts = JsonConvert.DeserializeObject<dynamic>(item.CustomerContacts ?? string.Empty)
                });
            }
            return response;
        }
    }
}
