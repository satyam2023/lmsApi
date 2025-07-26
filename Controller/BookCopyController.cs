using ECommerceApp.ApiResponse;
using lmsApi.Models.Dtos.BookCopy;
using lmsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lmsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookCopyController : ControllerBase
{
    private readonly IBookCopyServices _bookCopyService;

    public BookCopyController(IBookCopyServices bookCopyService)
    {
        _bookCopyService = bookCopyService;
    }
    [Authorize(Roles = "Admin")]
    [HttpPost("addBookCopy")]
    public async Task<IActionResult> AddBookCopy([FromBody] CreateBookCopy createBookCopy)
    {

        var response = await _bookCopyService.CreateBookCopy(createBookCopy);
        return StatusCode(response.StatusCode, response);
    }

    [Authorize]
    [HttpGet("getBookCopiesByBookId")]
    public async Task<IActionResult> GetBookCopiesByBookId([FromQuery] int bookId)
    {
        var response = await _bookCopyService.GetBookCopiesByBookId(bookId);
        return StatusCode(response.StatusCode, response);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("updateBookCopy/{id}")]
    public async Task<IActionResult> UpdateBookCopy(int id, [FromBody] UpdateBookCopyDto updateBookCopy)
    {

        var response = await _bookCopyService.UpdateBookCopy(id, updateBookCopy);
        return StatusCode(response.StatusCode, response);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("deleteBookCopy/{id}")]
    public async Task<IActionResult> DeleteBookCopy(int id)
    {
        var response = await _bookCopyService.DeleteBookCopy(id);
        return StatusCode(response.StatusCode, response);
    }

}
