using AquaWater.Domain.Commons;
using AquaWater.Dto.Request;
using AquaWater.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.BusinessLogic.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<Response<List<CompanyBasicResponseDTO>>> GetAllCompanies();
        SearchResponse<IEnumerable<CompanyResponseDTO>> GetAllCompaniesWithFeaturedProduct(SearchRequest<CompanySearchDTO> search);
        Task<Response<List<GetAllCompanyOrdersResponseDTO>>> GetAllCompanyOrders(string userId);
        Task<Response<string>> UpdateOrderStatus(string userId, string orderId, string status);
    }
}
