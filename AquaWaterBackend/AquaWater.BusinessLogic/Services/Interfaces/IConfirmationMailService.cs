using AquaWater.Dto.Response;
using System.Threading.Tasks;

namespace AquaWater.Data.Services.Interfaces
{
    public interface IConfirmationMailService
    {
        Task SendAConfirmationEmail(UserResponseDto user);
        Task SendAConfirmationEmailForResetPassword(UserResponseDto user);
        Task SendConfirmTokenEmail(string userId);
        Task<Response<string>> SendReminderEmail(string companyManagerId, string customerId);
    }
}