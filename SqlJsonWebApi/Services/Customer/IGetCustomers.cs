using System.Collections.Generic;
using System.Threading.Tasks;
using SqlJsonWebApi.Models;

namespace SqlJsonWebApi.Services.Customer
{
    public interface IGetCustomers
    {
        Task<List<Models.Customer>> Handler(Query query);
    }
}