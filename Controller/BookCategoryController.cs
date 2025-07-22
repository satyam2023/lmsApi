
using ECommerceApp.ApiResponse;
using lmsApi.Models.Entities;
using lmsApi.Models.Dtos.BookCategory;
using lmsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace lmsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookCategoryController : ControllerBase
{
    private readonly IBookCategoryService _bookCategoryService;

    public BookCategoryController(IBookCategoryService bookCategoryService)
    {
        _bookCategoryService = bookCategoryService;
    }

    [HttpPost("createBookCategory")]
    public async Task<IActionResult> CreateBookCategory([FromBody] CreateBookCategory category)
    {

        var response = await _bookCategoryService.CreateBookCategory(category);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("getAllCategories")]
    public async Task<IActionResult> GetAllCategories()
    {
        var response = await _bookCategoryService.GetAllCategories();
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("getActiveCategories")]
    public async Task<IActionResult> GetActiveCategories()
    {
        var response = await _bookCategoryService.GetActiveCategories();
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("getCategoryById/{id}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var response = await _bookCategoryService.GetCategoryById(id);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("updateCategory/{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateBookCategoryDto updateCategory)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid category ID");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _bookCategoryService.UpdateCategory(id, updateCategory);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("deleteCategory/{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var response = await _bookCategoryService.DeleteCategory(id);
        return StatusCode(response.StatusCode, response);
    }
}