using AquaWater.BusinessLogic.Services.Interfaces;
using AquaWater.Domain.Commons;
using AquaWater.Dto.Request;
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
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet(nameof(GetAllCompaniesWithFeaturedProduct))]
        public IActionResult GetAllCompaniesWithFeaturedProduct([FromQuery] SearchRequest<CompanySearchDTO> search)
        {
            try
            {
                if (!TryValidateModel(search))
                {
                    return BadRequest();
                }
                var result = _companyService.GetAllCompaniesWithFeaturedProduct(search);
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

        [HttpGet(nameof(GetAllCompanies))]
        public async Task<IActionResult> GetAllCompanies()
        {
            try
            {
                return Ok(await _companyService.GetAllCompanies());
            }
            catch (ArgumentException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return StatusCode(500, "An error occured we are working on it");
            }
        }


        [HttpGet(nameof(GetCompanyOrders))]
        public async Task<IActionResult> GetCompanyOrders()
        {
            try
            {
                var userid = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                return Ok(await _companyService.GetAllCompanyOrders(userid));
            }
            catch (ArgumentException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return StatusCode(500, "An error occured we are working on it");
            }
        }

        [HttpPost(nameof(UpdateOrderStatus))]
        public async Task<IActionResult> UpdateOrderStatus(string orderId, string status)
        {
            try
            {
                var userid = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                return Ok(await _companyService.UpdateOrderStatus(userid, orderId, status));
            }
            catch (ArgumentException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
                return StatusCode(500, "An error occured we are working on it");
            }
        }
    }
}
