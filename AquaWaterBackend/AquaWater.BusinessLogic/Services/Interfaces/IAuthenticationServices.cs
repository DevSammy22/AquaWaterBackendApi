using AquaWater.Dto.Request;
using AquaWater.Dto.Response;
using System.Threading.Tasks;

namespace AquaWater.BusinessLogic.Services.Implementations
{
    public interface IAuthenticationServices
    {
        Task<Response<string>> EmailConfirmationAsync(ConfirmEmailRequestDTO confirmEmailRequest);
        Task<Response<UserResponseDto>> LoginAsync(UserRequestDTO userRequest);
        Task<Response<string>> UpdatePassword(UpdatePasswordDTO updatePasswordDTO);
        Task<Response<string>> ForgotPasswordAsync(string email);
        Task<Response<string>> ResetPasswordAsync(ResetPasswordDTO resetPassword);
    
    }
}