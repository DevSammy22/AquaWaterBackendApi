using AquaWater.BusinessLogic.Services.Interfaces;
using AquaWater.Domain.Commons;
using AquaWater.Dto.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AquaWater.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet(nameof(GetProductsByCompanyID))]
        public async Task<IActionResult> GetProductsByCompanyID([FromQuery] SearchRequest<ProductSearchDTO> search)
        {
            try
            {
                if (!TryValidateModel(search))
                {
                    return BadRequest();
                }

                var result = await _productService.GetProductsByCompanyID(search);
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

        [HttpGet(nameof(GetProductById))]
        public async Task<IActionResult> GetProductById(string productId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var result = await _productService.GetProductByIdAsync(productId);

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


        [HttpPost(nameof(AddFavourite))]
        [Authorize(Policy = "RequireCustomerOnly")]
        public async Task<IActionResult> AddFavourite(string productId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var result = await _productService.AddFavoriteProductAsync(productId, userId);

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


        
        [HttpPost(nameof(CreateProduct))]
        public async Task<IActionResult> CreateProduct(ProductCreateRequestDTO productCreateRequestDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var result = await _productService.CreateProductAsync(productCreateRequestDTO, userId);

                if (result.Success)
                {
                    return BadRequest(result);

                }
                return Ok(result);

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

        [HttpPut(nameof(UpdateProduct))]
        public async Task<IActionResult> UpdateProduct(ProductUpdateRequest productUpdateRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var result = await _productService.UpdateProductAsync(productUpdateRequest, userId);

                if (result.Success)
                {
                    return BadRequest(result);

                }
                return Ok(result);

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

        [HttpDelete]
        [Route("Delete-Product")]
        [Authorize(Policy = "RequireCompanyManagerOnly")]
        public async Task<IActionResult> DeleteProductAsync(string productId)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;
                await _productService.DeleteProduct(productId, userId);
                return Ok();
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