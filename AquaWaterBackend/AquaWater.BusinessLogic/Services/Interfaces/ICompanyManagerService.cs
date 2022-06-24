using AquaWater.Dto.Common;
using AquaWater.Dto.Response;
using System.Threading.Tasks;

namespace AquaWater.Data.Repository.Interfaces
{
    public interface ICompanyManagerService
    {

        public Task<Response<string>> CreateCompanyManagerAsyn(CompanyManagerRequestDTO companyManagerRequest);

        public Task<Response<CompanyManagerOverviewResponseDTO>> GetOverView(string companyManagerId);
    }
}
