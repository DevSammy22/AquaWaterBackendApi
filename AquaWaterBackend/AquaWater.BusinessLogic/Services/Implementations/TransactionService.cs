using AquaWater.BusinessLogic.Services.Interfaces;
using AquaWater.BusinessLogic.Utilities;
using AquaWater.Data.Context;
using AquaWater.Data.Repository;
using AquaWater.Data.Services.Utilities;
using AquaWater.Domain.Entities;
using AquaWater.Dto.Response;
using AquaWater.Dto.TransactionDTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PayStack.Net;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AquaWater.BusinessLogic.Services.Implementations
{
    #region Constructor 
    public class TransactionService : ITransactionService
    {
        private readonly IGenericRepo<Transaction> _transactionRepository;
        private readonly IConfiguration _configuration;
        private readonly IGenericRepo<Customer> _customerRepository;
        private readonly IGenericRepo<Order> _orderRepository;
        private readonly IGenericRepo<User> _userRepository;
        private readonly IFindApplicationUser _applicationUserService;
        private PayStackApi Paystack;
        public TransactionService(IGenericRepo<Transaction> transanctionRepository, IConfiguration configuration, IGenericRepo<Customer> customerRepository, IGenericRepo<Order> orderRepository, IFindApplicationUser applicationUserService, IGenericRepo<User> userRepository)
        {
            _transactionRepository = transanctionRepository;
            _configuration = configuration;
            Paystack = new PayStackApi(_configuration["Payment:Paystack"]);
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _applicationUserService = applicationUserService;
            _userRepository = userRepository;
        }
        public async Task<Response<string>> MakePayment(string orderId, string userId)
        {
            var customer = await _applicationUserService.GetCustomerByUserIdAsync(userId);
            var order = _orderRepository.Table.Include(x => x.OrderItem).FirstOrDefault(x => x.Id == Guid.Parse(orderId));
            if (customer.Id != order.CustomerId)
            {
                throw new AccessViolationException("You are not authorized to access this resource");
            }

            decimal totalPrice = 0;
            foreach (var orderItem in order.OrderItem)
            {
                totalPrice += (orderItem.CurrentProductPrice * orderItem.Quantity);
            }
            TransactionInitializeRequest request = new()
            {
                AmountInKobo = (int)totalPrice * 100,
                Email = customer.User.Email,
                Reference = $"{Constants.TrxRef}-{DateTime.Now.ToString("yyyyyMMddHHmmss")}-{orderId.ToString()[..6]}",
                Currency = Constants.Currency,
                CallbackUrl = _configuration["Payment:Callbackurl"]
            };
            Response<string> response = new Response<string>();
            TransactionInitializeResponse transactionResponse = Paystack.Transactions.Initialize(request);
            if (transactionResponse.Status)
            {
                var transaction = new Transaction()
                {
                    OrderId = Guid.Parse(orderId),
                    Amount = totalPrice,
                    TransactionReference = request.Reference,
                    OrderDate = order.OrderDate,
                    UpdatedAt = DateTime.Now,
                    Description = "Payment for order: " + orderId,
                    Status = transactionResponse.Status,
                };

                await _transactionRepository.InsertAsync(transaction);
                response.Data = transactionResponse.Data.AuthorizationUrl;
                response.Success = true;
                response.Message = "Payment Link generated successfully:" + request.Reference;
            }
            else
            {
                response.Message = "Payment link could not be generated at this time please try again";
            }
            return response;
        }

        public async Task<Response<string>> PaymentVerification(string reference)
        {
            var transaction = _transactionRepository.Table.FirstOrDefault(x => x.TransactionReference == reference);
            if (transaction == null)
            {
                throw new ArgumentException("Invalid transaction reference: " + reference);
            }
            TransactionVerifyResponse Transactionresponse = Paystack.Transactions.Verify(reference);
            Response<string> response = new Response<string>();

            if (Transactionresponse.Data.Status == "success")
            {
                var order = _orderRepository.Table.Include(x => x.OrderItem).FirstOrDefault(x => x.Id == transaction.OrderId);
                if (order.OrderStatus != (int)OrderStatus.OnHold && order.OrderStatus != (int)OrderStatus.Failed)
                {
                    transaction.Status = false;
                    response.Message = "Transaction is either verified or declined";
                    return response;
                }
                if (transaction != null)
                {
                    transaction.Status = true;
                    await _transactionRepository.UpdateAsync(transaction);
                    order.OrderStatus = (int)OrderStatus.Paid;
                    order.OTP = Helpers.GenerateOrderToken();
                    await _orderRepository.UpdateAsync(order);
                    response.Success = true;
                    response.Message = "Payment successfully verified!";
                }
                else
                {
                    transaction.Status = false;
                    await _transactionRepository.UpdateAsync(transaction);
                    order.OrderStatus = (int)OrderStatus.Failed;
                    await _orderRepository.UpdateAsync(order);
                    response.Message = "Payment was not successfully verified";
                }
            }
            response.Data = Transactionresponse.Data.GatewayResponse;
            return response;
        }
        #endregion
        #region Implimentation

        public async Task<Response<string>> DeleteAllTransactionsAsync(string userId)
        {
            var customer = _customerRepository.Table.FirstOrDefault(x => x.UserId == userId);
            if (customer == null)
            {
                throw new ArgumentException("User not exist");
            }
            var transactions = _orderRepository.Table
                .Where(x => x.CustomerId == customer.Id)
                .Include(y => y.Transaction)
                .Select(x => x.Transaction.Id).ToList();

            foreach (var transactionId in transactions)
            {
                await _transactionRepository.DeleteAsync(transactionId);
            }
            return new Response<string>()
            {
                Message = "Your transactions have been successfully deleted",
                Success = true
            };
        }

        public async Task<Response<string>> DeleteTransactionByIdAsync(string transactionId, string userId)
        {

            var transaction = _transactionRepository.TableNoTracking.Where(d => d.Id == Guid.Parse(transactionId)).Include(t => t.Orders).FirstOrDefault();
            if (transaction == null)
            {
                throw new ArgumentException("Transaction not found");
            }

            var customer = await _applicationUserService.GetCustomerByUserIdAsync(userId);
            {
                if (transaction.Orders.CustomerId == customer.Id)
                {
                    await _transactionRepository.DeleteAsync(transaction.Id);
                    return new Response<string>()
                    {
                        Message = $"Your transaction with {transactionId} has been successfully deleted",
                        Success = true
                    };
                }
            }
            throw new ArgumentException($"Transction with {transactionId} not found");
        }
        #endregion
    }
}