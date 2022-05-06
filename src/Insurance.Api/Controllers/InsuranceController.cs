using Insurance.Business;
using Insurance.Business.DTOs;
using Insurance.Business.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Insurance.Api.Controllers
{
    [ApiController]
    public class InsuranceController : ControllerBase
    {
        private readonly ILogger<InsuranceController> _logger;
        private readonly IProductService _productService;

        public InsuranceController(ILogger<InsuranceController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpPost]
        [Route("product/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> CalculateInsurance(int id)
        {
            try
            {
                return Ok(await _productService.CalculateInsurance(id));
            }
            catch (ProductNotFoundException ex)
            {
                _logger.LogInformation(ex.Message, $"Product Not found {id}");
                return NotFound(id);
            }
        }

        [HttpPost]
        [Route("order")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> CalculateInsurance([FromBody] OrderDto ordertoInsure)
        {
            try
            {
                return Ok(await _productService.CalculateInsurance(ordertoInsure));
            }
            catch (ProductNotFoundException ex)
            {
                _logger.LogInformation(ex.Message, "Product Not found", ordertoInsure);
                return NotFound(ordertoInsure);
            } 
        }

        [HttpPost]
        //[Authorize]
        [Route("updateSurcharge")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> UpdateSurcharge([FromBody] ProductTypeDto productTypeDto)
        {
            try
            {
                return Ok(await _productService.UpdateSurcharge(productTypeDto));
            }
            catch (ProductTypeNotFoundException ex)
            {
                _logger.LogInformation(ex.Message, "Product Type Id Invalid", productTypeDto);
                return NotFound(productTypeDto);
            }
        }
    }
}