using AquaWater.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.BusinessLogic.Services.Interfaces
{
    public interface IFindApplicationUser
    {
        Task<Customer> GetCustomerByUserIdAsync(string id);
        Task<AdminUser> GetAdminByUserIdAsync(string id);
        Task<CompanyManager> GetCompanyManagerByUserIdAsync(string id);
    }
}
