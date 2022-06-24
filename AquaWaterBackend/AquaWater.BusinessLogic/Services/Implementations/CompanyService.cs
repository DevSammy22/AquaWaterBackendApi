using AquaWater.BusinessLogic.Services.Interfaces;
using AquaWater.BusinessLogic.Utilities;
using AquaWater.Data.Context;
using AquaWater.Data.Repository;
using AquaWater.Domain.Commons;
using AquaWater.Domain.Entities;
using AquaWater.Dto.Request;
using AquaWater.Dto.Response;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.BusinessLogic.Services.Implementations
{
    #region Construtor 
    public class CompanyService : ICompanyService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepo<Company> _companyRepo;
        private readonly IGenericRepo<OrderItem> _orderItemRepo;
        private readonly IFindApplicationUser _findApplicationUser;
        private readonly IGenericRepo<Order> _orderGenricRepo;
        private readonly IGenericRepo<Customer> _customerGenericRepo;

        public CompanyService(IMapper mapper, IGenericRepo<Company> companyRepo, IGenericRepo<OrderItem> orderItem, IFindApplicationUser findApplicationUser, IGenericRepo<Order> orderGenricRepo, IGenericRepo<Customer> customerGenericRepo)
        {
            _mapper = mapper;
            _companyRepo = companyRepo;
            _orderItemRepo = orderItem;
            _findApplicationUser = findApplicationUser;
            _orderGenricRepo = orderGenricRepo;
            _customerGenericRepo = customerGenericRepo;
        }

        #endregion

        #region Implementation 

        public SearchResponse<IEnumerable<CompanyResponseDTO>> GetAllCompaniesWithFeaturedProduct(SearchRequest<CompanySearchDTO> search)
        {
            if (search == null)
            {
                throw new ArgumentException($"Invalid search parameters");
            }
            var query = _companyRepo.TableNoTracking;

            if (search.Data != null)
            {
                if (string.IsNullOrWhiteSpace(search.Data.CompanyName))
                {
                    query = query.Where(x => x.CompanyName.Contains(search.Data.CompanyName));
                }
                if (string.IsNullOrWhiteSpace(search.Data.State))
                {
                    query = query.Where(x => x.Location.State.Contains(search.Data.State));
                }
                if (string.IsNullOrWhiteSpace(search.Data.Country))
                {
                    query.Where(x => x.Location.Country.Contains(search.Data.Country));
                }
            }

            var IsFeatured = query.Include(x => x.Product.Where(c => c.IsFeature)).ThenInclude(x => x.ProductGallery)
                .Include(x => x.Location)
                .OrderBy(x => x.Id);

            return Paginator.Pagination<Company, CompanyResponseDTO>(IsFeatured, _mapper, search.PageSize, search.Page);
        }

        public async Task<Response<List<CompanyBasicResponseDTO>>> GetAllCompanies()
        {
            var companies = await _companyRepo.GetAllAsync();
            return new Response<List<CompanyBasicResponseDTO>>
            {
                Data = _mapper.Map<List<CompanyBasicResponseDTO>>(companies),
                Success = true,
                Message = "Successful"
            };
        }

        public async Task<Response<List<GetAllCompanyOrdersResponseDTO>>> GetAllCompanyOrders(string userId)
        {
            var companyManager = await _findApplicationUser.GetCompanyManagerByUserIdAsync(userId);
            if (companyManager == null)
            {
                throw new ArgumentException($"Wrong comapny Manager Id: {userId}");
            }

            var query = _orderGenricRepo.TableNoTracking.Include(x => x.OrderItem).ThenInclude(x => x.Product).Select(z => new
            {
                customer = z.Customer.User,
                location = z.Customer.User.Location,
                orderDate = z.OrderDate,
                productGallery = z.OrderItem.Select(x => x.Product.ProductGallery).FirstOrDefault(),
                orderItems = z.OrderItem.Where(x => x.Product.CompanyId == companyManager.CompanyId)
            }).Where(x => x.orderItems.Any());

            List<GetAllCompanyOrdersResponseDTO> orders = new();

            if (query != null)
            {
                foreach (var item in query)
                {
                    GetAllCompanyOrdersResponseDTO orderItem = new()
                    {
                        CustomerName = item.customer.FirstName + " " + item.customer.LastName,
                        OrderLocation = item.location.City + ", " + item.location.State,
                        ProductName = item.orderItems.FirstOrDefault().Product.Name,
                        OrderDate = item.orderDate.ToString(),
                        PictureUrl = item.productGallery.FirstOrDefault().ImageUrl,
                        Quantity = item.orderItems.FirstOrDefault().Quantity
                    };
                    orders.Add(orderItem);
                }
            }

            return new Response<List<GetAllCompanyOrdersResponseDTO>>
            {
                Data = orders,
                Success = true,
                Message = "Successful"
            };
        }

        public async Task<Response<string>> UpdateOrderStatus(string userId, string orderId, string status)
        {
            var companyManager = await _findApplicationUser.GetCompanyManagerByUserIdAsync(userId);
            if (companyManager == null)
            {
                throw new ArgumentException($"Wrong comapny Manager Id: {userId}");
            }

            var order = await _orderGenricRepo.GetByIdAysnc(Guid.Parse(orderId));
            if (order == null)
            {
                throw new ArgumentException($"Wrong order Id: {orderId}");
            }


            var query = _orderGenricRepo.TableNoTracking.Include(x => x.OrderItem).Include(x => x.OrderItem.Where(x => x.Product.CompanyId == companyManager.CompanyId));
            if(query == null)
            {
                throw new ArgumentException($"Company Manager does not belong to company");
            }


            if (status.ToLower().Trim() == "delivered")
            {
                order.OrderStatus = 5;
                await _orderGenricRepo.UpdateAsync(order);
                return new Response<string>
                {
                    Success = true,
                    Message = $"Successful updated order status to {status}"
                };
            }
            else if (status.ToLower().Trim() == "tobedelivered")
            {
                order.OrderStatus = 6;
                await _orderGenricRepo.UpdateAsync(order);
                return new Response<string>
                {
                    Success = true,
                    Message = $"Successful updated order status to {status}"
                };
            }
            else
            {
                throw new ArgumentException($"Wrong status: {status}");
            }

        }
        #endregion
    }
}
