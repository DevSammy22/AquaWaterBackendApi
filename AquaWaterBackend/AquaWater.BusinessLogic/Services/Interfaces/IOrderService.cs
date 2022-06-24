using AquaWater.Dto.Request;
using AquaWater.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.BusinessLogic.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<Response<string>> VerifyOTP(VerifyOtpDTO verifyOtp);
        public Task<Response<OrderDTO>> CreateOrderAsync(OrderRequest createOrderRequest, string id);
        public  Task<OrderResponse> GetOrderByIdAsync(String id);
        Task<Response<string>> GetTransactionReferenceByOrderId(string orderId, string userId);
        Task<Response<UpdateOrderResponseData>> UpdateOrderAsync(UpdateOrderRequestDTO updateOrder);
        Task<Response<List<OrderHisttoryResponseDTO>>> GetOrderHistory(string userId);
    }
}
