using AquaWater.BusinessLogic.Utilities;
using AquaWater.Data.Services.Interfaces;
using AquaWater.Domain.Entities;
using AquaWater.Dto.Request;
using AquaWater.Dto.Response;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace AquaWater.BusinessLogic.Services.Implementations
{
    #region Constructor
    public class AuthenticationService : IAuthenticationServices
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IConfirmationMailService _confirmationMailService;
        private IMapper _mapper;
        public AuthenticationService(UserManager<User> userManager, ITokenGenerator tokenGenerator, IMapper mapper, IConfirmationMailService confirmationMailService)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
            _mapper = mapper;
            _confirmationMailService = confirmationMailService;
        }
        #endregion

    #region Implementation
        public async Task<Response<UserResponseDto>> LoginAsync(UserRequestDTO userRequest)
        {
            var user = await _userManager.FindByEmailAsync(userRequest.Email);
            if (user != null)
            {
                if (await _userManager.CheckPasswordAsync(user, userRequest.Password))
                {
                    if (user.EmailConfirmed)
                    {
                        var response = new Response<UserResponseDto>()
                        {
                            Data = new UserResponseDto
                            {
                                Email = user.Email,
                                FullName = $"{user.FirstName + " " + user.LastName}",
                                Id = user.Id,
                                Token = await _tokenGenerator.GenerateTokenAsync(user)
                            }
                        };
                        response.Message = "Login Successful";
                        response.Success = true;
                        return response;
                    }
                    throw new AccessViolationException("Kindly verify your email address to login");
                }
                throw new AccessViolationException("Invalid credentials");
            }
            throw new AccessViolationException("Invalid Credntials");
        }

        public async Task<Response<string>> EmailConfirmationAsync(ConfirmEmailRequestDTO confirmEmailRequest)
        {
            var user = await _userManager.FindByEmailAsync(confirmEmailRequest.EmailAddress);
            if (user != null)
            {
                var decodedToken = TokenConverter.DecodeToken(confirmEmailRequest.Token);
                var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

                if (result.Succeeded)
                {
                    var response = new Response<string>()
                    {
                        Message = "Email Confirmation was successful",
                        Success = true
                    };

                    return response;
                }
                throw new ArgumentException("Your email could not be confirmed");
            }
            throw new ArgumentException($"User with email '{confirmEmailRequest.EmailAddress}' not found");
        }

        public async Task<Response<string>> UpdatePassword(UpdatePasswordDTO updatePasswordDTO)
        {
            var user = await _userManager.FindByEmailAsync(updatePasswordDTO.Email);
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, updatePasswordDTO.OldPassword);
                if (passwordCheck)
                {
                    var result = await _userManager.ChangePasswordAsync
                                (user, updatePasswordDTO.OldPassword, updatePasswordDTO.NewPassword);

                    if (result.Succeeded)
                    {
                        return new Response<string>()
                        {
                            Success = true,
                            Message = "Password update successful",
                            Data = null
                        };
                    }
                    else
                    {
                        return new Response<string>()
                        {
                            Success = true,
                            Message = "Password update successful",
                            Errors = result.GetIdentityErrors()
                        };
                    }
                }
                return new Response<string>()
                {
                    Success = false,
                    Message = "Password update unsuccessful"
                };
            }
            return new Response<string>
            {
                Success = false,
                Message = $"user with email : {updatePasswordDTO.Email} does not exist"
            };
        }

        public async Task<Response<string>> ResetPasswordAsync(ResetPasswordDTO resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
            {
                new Response<string>
                {
                    Success = false,
                    Message = $"Email {resetPassword.Email} does not exist"
                };
            }

            var token = TokenConverter.DecodeToken(resetPassword.Token);
            var result = await _userManager.ResetPasswordAsync(user, token, resetPassword.NewPassword);

            if (result.Succeeded)
            {
                return new Response<string>
                {
                    Success = true,
                    Message = "Password has been reset successfully"
                };
            }

            return new Response<string>
            {
                Success = false,
                Message = " Password was not reset succesfully",
                Errors = result.GetIdentityErrors()
            };

        }

        public async Task<Response<string>> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var userResponse = _mapper.Map<UserResponseDto>(user);
            var response = new Response<string>
            {
                Success = false,
                Message = "A mail has been sent to the specified email address if it exists"
            };

            if (user == null)
            {
                response.Success = false;
                return response;
            }

            userResponse.FullName = $"{user.FirstName + " " + user.LastName}";
            userResponse.Token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _confirmationMailService.SendAConfirmationEmailForResetPassword(userResponse);

            response.Success = true;
            return response;
        }
    }
    #endregion
}