using AquaWater.BusinessLogic.Services.Interfaces;
using AquaWater.Data.Repository.Interfaces;
using AquaWater.Data.Services.Implementations;
using AquaWater.Data.Services.Interfaces;
using AquaWater.Domain.Entities;
using AquaWater.Dto.Common;
using AquaWater.Dto.Response;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Transactions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AquaWater.BusinessLogic.Services.Interfaces;

namespace AquaWater.Data.Repository.Implementations
{
    #region Constructor 
    public class CompanyManagerService : ICompanyManagerService
    {
        private readonly IGenericRepo<CompanyManager> _companyManagerRepo;
        private readonly IGenericRepo<Company> _companyRepo;
        private readonly IGenericRepo<Order> _orderRepo;
        private readonly IGenericRepo<Customer> _customerRepo;
        private readonly IGenericRepo<CustomerFavourite> _customerFavouriteRepo;
        private readonly IGenericRepo<Product> _productRepo;
        private readonly IConfirmationMailService _confirmationMailService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IMailService _mailService;
        private readonly IFindApplicationUser _findUser;


        public CompanyManagerService(IGenericRepo<CompanyManager> companyManagerRepo, IConfirmationMailService confirmationMailService,
            IMapper mapper, IUserService userService, IMailService mailService, IGenericRepo<Company> companyRepo, 
            IGenericRepo<Order> orderRepo, IGenericRepo<Customer> customerRepo, IGenericRepo<CustomerFavourite> customerFavouriteRepo,
            IGenericRepo<Product> productRepo, IFindApplicationUser findUser)
        {
            _companyManagerRepo = companyManagerRepo;
            _confirmationMailService = confirmationMailService;
            _mapper = mapper;
            _userService = userService;
            _mailService = mailService;
            _companyRepo = companyRepo;
            _orderRepo = orderRepo;
            _customerRepo = customerRepo;
            _customerFavouriteRepo = customerFavouriteRepo;
            _productRepo = productRepo;
            _findUser = findUser;
        }

        #endregion
        #region Implimentation
        public async Task<Response<string>> CreateCompanyManagerAsyn(CompanyManagerRequestDTO companyManagerRequest)
        {
            
            var company = await _companyRepo.GetByIdAysnc(companyManagerRequest.CompanyId);
            if (company == null)
            {
                throw new ArgumentException($"Company with Id {companyManagerRequest.CompanyId} does not exist");
            }

            var response = new Response<string>();
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var userDetails = await _userService.RegisterAsync(companyManagerRequest.User);

                var template = _mailService.GetEmailTemplate("EmailTemplate.html");
                TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;
                var userName = textInfo.ToTitleCase(userDetails.FullName);

                await _confirmationMailService.SendAConfirmationEmail(userDetails);

                var companyManager = new CompanyManager()
                {
                    CompanyId = company.Id,
                    UserId = userDetails.Id
                };

                await _companyManagerRepo.InsertAsync(companyManager);

                transaction.Complete();
            }

            response.Success = true;
            response.Message = "Manager Registered Successful, Please comfirm email";
            return response;
        }

        public async Task<Response<CompanyManagerOverviewResponseDTO>> GetOverView(string userId)
        {

            var companyManager = await _findUser.GetCompanyManagerByUserIdAsync(userId);
            

            if (companyManager == null)
            {
                throw new ArgumentException($"CompanyManager with Id {companyManager.Id} does not exist");
            }
            var orderReceived = _orderRepo.TableNoTracking.Include(x => x.OrderItem).ThenInclude(x => x.Product)
                .ThenInclude(x => x.Company)
                .SelectMany(x=>x.OrderItem
                .Where(x=>x.Product.CompanyId == companyManager.CompanyId)).Count();
            var customers = _customerRepo.TableNoTracking.Include(x => x.User).Where(x => x.UserId == companyManager.UserId).Count();
            var customerFavourites = _orderRepo.TableNoTracking.Include(x => x.OrderItem).ThenInclude(x => x.Product)
                        .Select(z => new {
                            customerId = z.CustomerId,
                            orderItems = z.OrderItem
                        .Where(x => x.Product.CompanyId == companyManager.CompanyId)
                        })
                        .Where(x => x.orderItems.Any()).ToList();
                     var customerFaveNumber = customerFavourites.GroupBy(x => x.customerId)
                        .Where(c => c.Count() >= 2)
                        .Count();
            var totalSupplies = _orderRepo.TableNoTracking.Select(x => x.OrderStatus).Where(x => x == 1).Count();

            var response = new Response<CompanyManagerOverviewResponseDTO>
            {
                Data = new CompanyManagerOverviewResponseDTO()
                {
                    TotalSupplies = totalSupplies,
                    TotalOrdersReceived = orderReceived,
                    FavouriteCustomers = customerFaveNumber,
                    Customers = customers
                },
                Message = $"Overview of the {companyManager.Id}",
                Success = true
            };
            return response;
        }
        #endregion
    }
}
