using Microsoft.AspNetCore.Mvc;
using RestApiPractice.LogicLayer.Interfaces;
using RestApiPractice.Models;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace RestApiPractice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {        
        private readonly IProductService _productService;
        private readonly IConfiguration _configuration;

        public ProductsController(IProductService productService, IConfiguration configuration)
        {
            _productService = productService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductModel product)
        {
            await _productService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductModel product)
        {
            var updated = await _productService.UpdateProductAsync(id, product);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleted = await _productService.DeleteProductAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet("test-connection")]
        public IActionResult TestConnection()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                using var connection = new SqlConnection(connectionString);
                connection.Open();

                return Ok("資料庫連線成功！");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"資料庫連線失敗: {ex.Message}");
            }
        }

    }
}
// curl -v http://localhost:80/api/products 