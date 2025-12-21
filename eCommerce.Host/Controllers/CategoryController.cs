using eCommerce.Application.DTOs.Category;
using eCommerce.Application.DTOs.Product;
using eCommerce.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //private readonly + send in constructor 
    //or send as a parameter in controller signature(primary constructor) (C# 12.0 feature)
    public class CategoryController(ICategoryService CategoryService) : ControllerBase
    {
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var data = await CategoryService.GetAllAsync();
            return data.Any() ? Ok(data) : NotFound(new { message = "there is no found data" });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await CategoryService.GetByIdAsync(id);
            return data != null ? Ok(data) : NotFound(new { message = $"item with id {id} is not found" });
        }
        [HttpPost("add")]
        public async Task<IActionResult> Add(CreateCategoryDto Category)
        {
            //to ensure that the model sent by user is applying all required rules in dtos(data annotation)
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await CategoryService.AddAsync(Category);
            return result.success ? Ok(result) : StatusCode(500, new { message = "adding Category failed" });
        }
        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateCategoryDto Category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await CategoryService.UpdateAsync(Category);
            return result.success ? Ok(result) : StatusCode(500, new { message = "updating Category failoed" });
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await CategoryService.DeleteAsync(id);
            return result.success ? NoContent() : StatusCode(500, new { message = "deleting Category failed" });
        }
    }
}
