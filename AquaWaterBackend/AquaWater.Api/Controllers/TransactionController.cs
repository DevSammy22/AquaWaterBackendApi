using AquaWater.BusinessLogic.Services.Interfaces;
using AquaWater.Data.Services.Interfaces;
using AquaWater.Dto.Response;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AquaWater.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IConfirmationMailService _confirmationMailService;
        private readonly IOrderService _orderService;
    
        public TransactionController(ITransactionService transactionService, IConfirmationMailService confirmationMailService, IOrderService orderService)
        {
            _transactionService = transactionService;
            _confirmationMailService = confirmationMailService;
            _orderService = orderService;
        }

        [HttpPost]
        [Route(nameof(MakePayment))]
        [Authorize(Policy = "RequireCustomerOnly")]
        public async Task<IActionResult> MakePayment(string orderId)
        {
            var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
            try
            {
               Response<string> response =  await _transactionService.MakePayment(orderId, userId);
                if (response.Success) 
                { 
                    return Ok(response); 
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured we are working on it");
            }
        }

        [HttpPost]
        [Route(nameof(PaymentVerification))]
        public async Task<IActionResult> PaymentVerification(string reference)
        {
            try
            {
                Response<string> response = await _transactionService.PaymentVerification(reference);
      
                if (response.Success)
                {
                   var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
                   await _confirmationMailService.SendConfirmTokenEmail(userId);
                    return Ok(response);
                }
                return BadRequest(response);
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

        [HttpPost]
        [Route(nameof(GetTransactionByOrderId))]
        public async Task<IActionResult> GetTransactionByOrderId(string orderId)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

                Response<string> response = await _orderService.GetTransactionReferenceByOrderId(orderId, userId);

                if (response.Success)
                {
                    return Ok(response);
                }
                return BadRequest(response);
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
       
        [HttpDelete]
        [Route(nameof(DeleteAllTransaction))]
        [Authorize(Policy = "RequireCustomerOnly")]
        public async Task<IActionResult> DeleteAllTransaction()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var result = await _transactionService.DeleteAllTransactionsAsync(userId);

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
      

        [HttpDelete]
        [Route(nameof(DeleteTransactionsById))]
        [Authorize(Policy = "RequireCustomerOnly")]
        public async Task<IActionResult> DeleteTransactionsById(string transactionId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var result = await _transactionService.DeleteTransactionByIdAsync(transactionId, userId);

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
    }
}
