using AquaWater.BusinessLogic.Services.Interfaces;
using AquaWater.BusinessLogic.Utilities;
using AquaWater.Data.Repository;
using AquaWater.Data.Services.Interfaces;
using AquaWater.Data.Repository.Interfaces;
using AquaWater.Domain.Commons;
using AquaWater.Domain.Entities;
using AquaWater.Dto.Request;
using AquaWater.Dto.Response;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace AquaWater.Data.Services.Implementations
{
    #region Constructor
    public class CustomerService : ICustomerService
    {
        private readonly IGenericRepo<Customer> _customerRepo;
        private readonly IConfirmationMailService _confirmationMailService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IFindApplicationUser _findApplicationUser;
        private readonly IGenericRepo<Order> _orderGenericRepo;
        private readonly IGenericRepo<OrderItem> _orderItemGenericRepo;

        public CustomerService(IGenericRepo<Customer> genericRepo, IConfirmationMailService confirmationMailService,
            IMapper mapper, IUserService userService,
            IFindApplicationUser applicationUser, IGenericRepo<Order> orderGenericRepo,
            IGenericRepo<OrderItem> orderItemGenericRepo)
        {
            _customerRepo = genericRepo;
            _confirmationMailService = confirmationMailService;
            _mapper = mapper;
            _userService = userService;
            _findApplicationUser = applicationUser;
            _orderGenericRepo = orderGenericRepo;
            _orderItemGenericRepo = orderItemGenericRepo;
        }

        #endregion

        #region Implimentation
        public async Task<Response<string>> CreateCustomer(CustomerRequestDTO customerRequestDTO)
        {
            var response = new Response<string>();
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var userDetails = await _userService.RegisterAsync(customerRequestDTO.User);

                TextInfo textInfo = new CultureInfo("en-GB", false).TextInfo;
                var userName = textInfo.ToTitleCase(userDetails.FullName);

                await _confirmationMailService.SendAConfirmationEmail(userDetails);

                var customer = new Customer()
                {
                    UserId = userDetails.Id,
                    EarnedCash = customerRequestDTO.EarnedCash,
                    ConsumptionLevel = customerRequestDTO.ConsumptionLevel
                };

                await _customerRepo.InsertAsync(customer);

                transaction.Complete();
            }

            response.Success = true;
            response.Message = "Customer Registered Successful, Please comfirm email";
            return response;
        }


        public async Task<SearchResponse<IEnumerable<CustomerTransactionDTO>>> GetCustomerTransaction(SearchRequest<string> searchRequest, string userId)
        {
            var customer = await _findApplicationUser.GetCustomerByUserIdAsync(userId);
            var orders = _customerRepo.TableNoTracking.Where(x => x.Id == customer.Id)
                            .Include(c => c.Orders).ThenInclude(o => o.OrderItem)
                            .ThenInclude(o => o.Product).SelectMany(x => x.Orders);

            var transactions = from o in orders
                               select new CustomerTransactionDTO
                               {
                                   Id = o.Id,
                                   Amount = o.OrderItem.Sum(x => x.CurrentProductPrice),
                                   Description = o.OrderItem.First().Product.Name,
                                   OrderStatusId = o.OrderStatus,
                                   OrderDate = o.OrderDate,
                                   UpdatedAt = o.UpdatedAt,
                               };

            return Paginator.Pagination(transactions, searchRequest.PageSize, searchRequest.Page);
        }
        #endregion
    }
}
