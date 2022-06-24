using AquaWater.BusinessLogic.Services.Implementations;
using AquaWater.Data.Repository.Interfaces;
using AquaWater.Data.Services.Interfaces;
using AquaWater.Dto.Common;
using AquaWater.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AquaWater.Api.Controllers
{
    [Route("api/v1/[Controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationServices _authenticationServices;
        private readonly IUserService _userService;
        private readonly IConfirmationMailService _confirmationMailService;

        public AccountController(IAuthenticationServices authenticationServices, IUserService userService, IConfirmationMailService confirmationMailService)
        {
            _authenticationServices = authenticationServices;
            _userService = userService;
            _confirmationMailService = confirmationMailService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserRequestDTO userRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _authenticationServices.LoginAsync(userRequest);
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }


            catch (AccessViolationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured we are working on it");
            }
        }

        [HttpPost]
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync([FromBody] ConfirmEmailRequestDTO confirmEmailRequestDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _authenticationServices.EmailConfirmationAsync(confirmEmailRequestDTO);
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                return Ok(result);

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured we are working on it");
            }
        }

        [HttpPatch("UpdatePassword")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordDTO updatePasswordDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _authenticationServices.UpdatePassword(updatePasswordDTO);

                if (!result.Success)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured we are working on it");
            }
        }

        [HttpPost("Reset-Password")]

        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordDTO resetPassword)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _authenticationServices.ResetPasswordAsync(resetPassword);
                if (!result.Success)
                {
                    return BadRequest(result);

                }
                return Ok(result);
            }
            catch (ArgumentNullException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred we are working on it");
            }
        }

        [HttpPost("Forgot-Password")]

        public async Task<IActionResult> ForgotPasswordAsync(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return NotFound();
                }
                var result = await _authenticationServices.ForgotPasswordAsync(email);
                if (result.Success)
                {
                    return Ok(result);
                }
            }
            catch (ArgumentNullException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
            }
            return BadRequest(400);
        }

        [HttpPatch(nameof(EditUser))]
        [Authorize]
        public async Task<IActionResult> EditUser([FromBody] EditUserDTO model)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var result = await _userService.EditUser(userId, model);
                return Ok(result);
            }
            catch (ArgumentException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured we are working on it");
            }
        }


        [HttpPost]
        [Route("SendReminderEmail")]
        public async Task<IActionResult> SendReminderEmailAsync(string customerId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var result = await _confirmationMailService.SendReminderEmail(userId, customerId);
                  
                if (!result.Success)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }


            catch (AccessViolationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured we are working on it");
            }
        }

    }
}