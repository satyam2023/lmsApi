
using lmsApi.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookServices _bookServices;

    public BookController(IBookServices bookServices)
    {
        _bookServices = bookServices;
    }
    [Authorize(Roles = "Admin")]
    [HttpPost("addBook")]
    public async Task<IActionResult> AddBook([FromBody] CreateBook bookDto)
    {
        var result = await _bookServices.CreateBook(bookDto);
        return StatusCode((int)result.StatusCode, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("updateBook")]
    public async Task<IActionResult> UpdateBook([FromQuery] int bookId, [FromBody] UpdateBookRequest updateBookDto)
    {
        var result = await _bookServices.UpdateBookDetail(updateBookDto, bookId);
        return StatusCode((int)result.StatusCode, result);
    }

}