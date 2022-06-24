using AquaWater.Dto.Response;
using AquaWater.Dto.TransactionDTO;
using System.Threading.Tasks;

namespace AquaWater.BusinessLogic.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<Response<string>> DeleteAllTransactionsAsync(string userId);
        Task<Response<string>> DeleteTransactionByIdAsync(string transactionId, string userId);
        Task<Response<string>> MakePayment(string orderId, string userId);
        Task <Response<string>> PaymentVerification(string reference);
    }
}