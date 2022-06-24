using AquaWater.BusinessLogic.Services.Interfaces;
using AquaWater.Data.Repository;
using AquaWater.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AquaWater.BusinessLogic.Services.Implementations
{
    public class FindApplicationUser : IFindApplicationUser
    {
        #region Construtor 
        private readonly IGenericRepo<Customer> _customerGenericRepo;
        private readonly IGenericRepo<AdminUser> _adminGenericRepo;
        private readonly IGenericRepo<CompanyManager> _companyMangerGenericRepo;

        public FindApplicationUser(IGenericRepo<Customer> customerGenericRepo, IGenericRepo<AdminUser> adminGenericRepo,
            IGenericRepo<CompanyManager> companyMangerGenericRepo)
        {
            _customerGenericRepo = customerGenericRepo;
            _adminGenericRepo = adminGenericRepo;
            _companyMangerGenericRepo = companyMangerGenericRepo;
        }
        #endregion
# region Implimentation

        public async Task<Customer> GetCustomerByUserIdAsync(string id)
        {
            var customer = await _customerGenericRepo.TableNoTracking
                            .Where(x => x.UserId == id)
                            .Include(x => x.User).FirstOrDefaultAsync();
            return customer;
        }

        public async Task<AdminUser> GetAdminByUserIdAsync(string id)
        {
            var adminUser = await _adminGenericRepo.TableNoTracking
                                .Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == id);
            return adminUser;
        }

        public async Task<CompanyManager> GetCompanyManagerByUserIdAsync(string id)
        {
            var companyManager = await _companyMangerGenericRepo.TableNoTracking
                                    .Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == id);
            return companyManager;
        }
        #endregion
    }
}
