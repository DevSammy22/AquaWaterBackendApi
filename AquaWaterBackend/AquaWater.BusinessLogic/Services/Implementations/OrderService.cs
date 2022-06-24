using AquaWater.BusinessLogic.Services.Interfaces;
using AquaWater.BusinessLogic.Utilities;
using AquaWater.Data.Repository;
using AquaWater.Domain.Entities;
using AquaWater.Dto.Request;
using AquaWater.Dto.Response;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquaWater.BusinessLogic.Services.Implementations
{
    public class OrderService : IOrderService
    {
        #region Constructor
        private readonly IGenericRepo<Order> _orderRepository;
        private readonly IGenericRepo<Product> _productRepository;
        private readonly IGenericRepo<Transaction> _transactionRepository;
        private readonly IFindApplicationUser _findAppUser;
        private readonly IMapper _mapper;
        private readonly IGenericRepo<Order> _orderGenericRepo;
        private readonly IGenericRepo<OrderItem> _orderItemGenericRepo;

        public OrderService(IGenericRepo<Order> genericRepo,
            IMapper mapper, IGenericRepo<Product> productRepository,
            IFindApplicationUser findAppUser, IGenericRepo<Transaction> transactionRepository, IGenericRepo<Order> orderGenericRepo, IGenericRepo<OrderItem> orderItemGenericRepo)
        {
            _orderRepository = genericRepo;
            _mapper = mapper;
            _productRepository = productRepository;
            _findAppUser = findAppUser;
            _transactionRepository = transactionRepository;
            _orderGenericRepo = orderGenericRepo;
            _orderItemGenericRepo = orderItemGenericRepo;
        }
        #endregion

        #region Implementation
        public async Task<OrderResponse> GetOrderByIdAsync(string id)
        {
            var order = await _orderRepository.TableNoTracking
                            .Where(o => o.Id == Guid.Parse(id))
                            .Include(o => o.OrderItem).FirstOrDefaultAsync();
            if (order == null)
            {
                throw new ArgumentException($"Order with Id {id} doesn't exist");
            }

            var map = _mapper.Map<OrderResponse>(order);
            return map;
        }

        public async Task<Response<string>> GetTransactionReferenceByOrderId(string orderId, string userId)
        {
            var isValidGuid = Guid.TryParse(orderId, out Guid orderIdGuid);
            if (!isValidGuid)
                throw new ArgumentException($"OrderId {orderId} is invalid");

            var order = await _orderRepository.GetByIdAysnc(orderIdGuid);
            if (order == null)
            {
                throw new ArgumentException("Invalid orderId");
            }
            var customer = await _findAppUser.GetCustomerByUserIdAsync(userId);

            Response<string> response = new Response<string>();
            response.Message = "Payment not initialized";
            if (customer.Id == order.CustomerId)
            {
                var transaction = _transactionRepository.Table.FirstOrDefault(x => x.OrderId == Guid.Parse(orderId));
                response.Data = transaction.TransactionReference;
                response.Success = true;
                response.Message = "Successful";
            }
            else
            {
                response.Message = $"OrderId: {orderId} does not exist for this customer";
            }
            return response;
        }

        public async Task<Response<OrderDTO>> CreateOrderAsync(OrderRequest createOrderRequest, string id)
        {
            var customer = await _findAppUser.GetCustomerByUserIdAsync(id);
            Order order = _mapper.Map<Order>(createOrderRequest);
            order.CustomerId = customer.Id;
            order.OrderStatus = (int)OrderStatus.OnHold;
            order.OrderDate = DateTime.Now;

            string error = string.Empty;
            foreach (var item in createOrderRequest.OrderItem)
            {
                var product = await _productRepository.GetByIdAysnc(item.ProductId);
                if (product == null)
                {
                    error += $"Product with Id {product.Id} \\n\\";
                    continue;
                }

                var orderItem = order.OrderItem.FirstOrDefault(x => x.ProductId == item.ProductId);

                orderItem.CurrentProductPrice = await GetProductPriceAsync(item.ProductId);
                orderItem.Quantity = item.Quantity;
            }

            await _orderRepository.InsertAsync(order);
            return new Response<OrderDTO>()
            {
                Errors = error,
                Message = "Successfully",
                Success = true,
                Data = _mapper.Map<OrderDTO>(order)
            };
        }


        public async Task<Response<string>> VerifyOTP(VerifyOtpDTO verifyOtp)
        {
            var order = await _orderRepository.GetByIdAysnc(Guid.Parse(verifyOtp.OrderId));
            if (order.OTP != verifyOtp.OTP)
            {
                return new Response<string>
                {
                    Success = false,
                    Message = "OTP verification unsuccessful"
                };
            }
            return new Response<string>
            {
                Success = true,
                Message = "OTP verification successful"
            };
        }

        public async Task<Response<UpdateOrderResponseData>> UpdateOrderAsync(UpdateOrderRequestDTO updateOrder)
        {
            var isValidGuid = Guid.TryParse(updateOrder.OrderID, out Guid orderId);

            if (!isValidGuid)
                throw new ArgumentException($"OrderId {updateOrder.OrderID} is invalid");

            using (var transaction = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeAsyncFlowOption.Enabled))
            {
                var order = await _orderGenericRepo.TableNoTracking.Where(o => o.Id == orderId)
                .Include(o => o.OrderItem).FirstOrDefaultAsync();

                foreach (var item in order.OrderItem)
                {
                    await _orderItemGenericRepo.DeleteAsync(item);
                }
                order.UpdatedAt = DateTime.Now;

                var orderItems = _mapper.Map<List<OrderItem>>(updateOrder.OrderItems);

                foreach (var item in orderItems)
                {
                    item.OrderId = orderId;
                    item.CurrentProductPrice = await GetProductPriceAsync(item.ProductId);
                    await _orderItemGenericRepo.InsertAsync(item);
                }

                var response = new Response<UpdateOrderResponseData>()
                {
                    Errors = null,
                    Message = "Successful",
                    Success = true,
                    Data = _mapper.Map<UpdateOrderResponseData>(order)
                };

                transaction.Complete();

                return response;
            }
        }

        public async Task<Response<List<OrderHisttoryResponseDTO>>> GetOrderHistory(string userId)
        {
            var companyManager = await _findAppUser.GetCompanyManagerByUserIdAsync(userId);
            if (companyManager == null)
            {
                throw new ArgumentException("Invalid Company Manager");
            }

            var query = _orderGenericRepo.TableNoTracking.Include(x => x.OrderItem).ThenInclude(x => x.Product).Select(z => new
            {
                orderStatus = z.OrderStatus,
                orderDate = z.OrderDate,
                productGallery = z.OrderItem.Select(x => x.Product.ProductGallery).FirstOrDefault(),
                orderItems = z.OrderItem.Where(x => x.Product.CompanyId == companyManager.CompanyId)
            }).Where(x => x.orderItems.Any());

            List<OrderHisttoryResponseDTO> list = new();
            if(query != null)
            {
            foreach (var item in query)
            {
                    string orderStatusString = item.orderStatus == (int)OrderStatus.OnHold ? OrderStatus.OnHold.ToString()
                                            : item.orderStatus == (int)OrderStatus.Paid ? OrderStatus.Paid.ToString()
                                            : item.orderStatus == (int)OrderStatus.Declined ? OrderStatus.Declined.ToString()
                                            : item.orderStatus == (int)OrderStatus.Failed ? OrderStatus.Failed.ToString() : OrderStatus.Delivered.ToString();

                var orderHistory = new OrderHisttoryResponseDTO
                {
                    OrderDate = item.orderDate,
                    OrderStatus = orderStatusString,
                    ProductName = item.orderItems.FirstOrDefault().Product.Name,
                    ImageUrl = item.productGallery.FirstOrDefault().ImageUrl,
                    Quantity = item.orderItems.FirstOrDefault().Quantity,
                };
                list.Add(orderHistory);
            }

            }
                return new Response<List<OrderHisttoryResponseDTO>>
                {
                    Success = true,
                    Message = "Successful",
                    Data = list
                };
            
            
        }
        #endregion

        #region Private
        private async Task<decimal> GetProductPriceAsync(Guid productId)
        {
            var product = await _productRepository.GetByIdAysnc(productId);
            if (product == null)
            {
                throw new ArgumentException($"Product with Id {product.Id} does not exist \\n\\");

            }
            return product.Price - (product.Price * (product.Discount / 100));
        }
        #endregion
    }
}
