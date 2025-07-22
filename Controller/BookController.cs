
using lmsApi.Models.Entities;
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

    [HttpPost("addBook")]
    public async Task<IActionResult> AddBook([FromBody] CreateBookDto bookDto)
    {
        var result = await _bookServices.CreateBook(bookDto);
        return StatusCode((int)result.StatusCode, result);
    }

}