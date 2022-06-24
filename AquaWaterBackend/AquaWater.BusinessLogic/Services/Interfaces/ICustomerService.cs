using AquaWater.Domain.Commons;
using AquaWater.Dto.Request;
using AquaWater.Dto.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AquaWater.Data.Services.Interfaces
{
    public interface ICustomerService
    {
        public Task<Response<string>> CreateCustomer(CustomerRequestDTO customerRequestDTO);
        public Task<SearchResponse<IEnumerable<CustomerTransactionDTO>>> GetCustomerTransaction(SearchRequest<string> searchRequest, string userId);
    }
}
