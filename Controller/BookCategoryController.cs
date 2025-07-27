
using lmsApi.ApiResponse;
using lmsApi.Models.Entities;
using lmsApi.Models.Dtos.BookCategory;
using lmsApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace lmsApi.Controllers;

[ApiController]
[Route("lmsApi/book-categories")]
public class BookCategoryController : ControllerBase
{
    private readonly IBookCategoryService _bookCategoryService;

    public BookCategoryController(IBookCategoryService bookCategoryService)
    {
        _bookCategoryService = bookCategoryService;
    }

    [Authorize(Roles = "Admin")]
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

     [Authorize(Roles = "Admin")]
    [HttpPut("updateCategory/{id}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateBookCategoryDto updateCategory)
    {
        var response = await _bookCategoryService.UpdateCategory(id, updateCategory);
        return StatusCode(response.StatusCode, response);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("deleteCategory/{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var response = await _bookCategoryService.DeleteCategory(id);
        return StatusCode(response.StatusCode, response);
    }
}