using AquaWater.Data.Services.Interfaces;
using AquaWater.Dto.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AquaWater.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IImageService _imageService;
        public FileController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPatch("image-upload")]
        public async Task<IActionResult> UploadImage([FromForm] AddImageDTO imageDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var upload = await _imageService.UploadAsync(imageDto.Image);
                var result = new ImageAddedDTO()
                {
                    PubliId = upload.PublicId,
                    Url = upload.Url.ToString()
                };
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
