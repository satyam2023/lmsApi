using lmsApi.Models.Dtos.IssuedBook;
using lmsApi.Services;
using ECommerceApp.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace lmsApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class IssuedBookController : ControllerBase
{
    private readonly IIssuedBookService _issuedBookService;

    public IssuedBookController(IIssuedBookService issuedBookService)
    {
        _issuedBookService = issuedBookService;
    }

    [HttpPost("issue")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> IssueBook([FromBody] IssueBookDto dto,[FromHeader(Name = "Authorization")] string authorization)
    {
         string token = authorization.Substring("Bearer ".Length).Trim();
        var result = await _issuedBookService.IssueBook(dto, token);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("submit")]
    public async Task<IActionResult> SubmitBook([FromBody] SubmitBookDto dto)

    {
        var result = await _issuedBookService.SubmitBook(dto);
        return StatusCode(result.StatusCode, result);
    }
}
