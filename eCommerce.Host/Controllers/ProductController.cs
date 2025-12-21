using eCommerce.Application.DTOs.Product;
using eCommerce.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService productService) : ControllerBase
    {
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var data = await productService.GetAllAsync();
            return Ok(data);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await productService.GetByIdAsync(id);
            return data != null ? Ok(data) : NotFound(new {message = $"item with id {id} is not found"});
        }
        [HttpPost("add")]
        public async Task<IActionResult> Add(CreateProductDto product)
        {
            //to ensure that the model sent by user is applying all required rules in dtos(data annotation)
            //if(!ModelState.IsValid)
            //    return BadRequest(ModelState);
            var result = await productService.AddAsync(product);
            return result.success ? Ok(result) : StatusCode(500, new {message = "adding product failed"});
        }
        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateProductDto product)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);
            var result = await productService.UpdateAsync(product);
            return result.success ? Ok(result) : StatusCode(500, new {message = "updating product failoed"});
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await productService.DeleteAsync(id);
            return result.success ? NoContent() : StatusCode(500, new {message = "deleting product failed"});
        }
    }

}
