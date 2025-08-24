using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Backend.DTOs;
using Backend.Repositories;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessController(IBusinessRepository businessRepository) : Controller
    {
        [HttpPost("CreateBusiness")]
        [Authorize]
        public async Task<IActionResult> CreateBusiness(CreateBusinessDTO createBusinessDTO)
        {
            await businessRepository.CreateBusiness(createBusinessDTO);
            return Ok();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetBusinessById(string userId)
        {
            var response = await businessRepository.GetBusinessByIdAsync(userId);
            return Ok(response);
        }
    }
}
