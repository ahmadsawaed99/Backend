using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Repositories;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository) 
        { 
            _productRepository = productRepository;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post(Product product)
        {
            var id = await _productRepository.AddPoductToDatabase(product);
            return Ok(id);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            var products = _productRepository.GetAll();
            return Ok(products);
        }


    }
}
