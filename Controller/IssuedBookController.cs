using lmsApi.Models.Dtos.IssuedBook;
using lmsApi.Services;
using lmsApi.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace lmsApi.Controller;

[ApiController]
[Route("lmsApi/issued-books")]
public class IssuedBookController : ControllerBase
{
    private readonly IIssuedBookService _issuedBookService;

    public IssuedBookController(IIssuedBookService issuedBookService)
    {
        _issuedBookService = issuedBookService;
    }

    [HttpPost("issue")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> IssueBook([FromBody] IssueBook dto, [FromHeader(Name = "Authorization")] string authorization)
    {
        string token = authorization.Substring("Bearer ".Length).Trim();
        var result = await _issuedBookService.IssueBook(dto, token);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("submit")]
    public async Task<IActionResult> SubmitBook([FromBody] SubmitBook dto)

    {
        var result = await _issuedBookService.SubmitBook(dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("getIssuedBooks")]
    public async Task<IActionResult> GetIssuedBooksToUser([FromQuery] Guid userId)
    {
        var result = await _issuedBookService.GetIssuedBooksToUser(userId);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("extendSubmissionDate/{issueId}")]
    public async Task<IActionResult> ExtendBookSubmissionDate([FromRoute] int issueId, [FromQuery] DateOnly extendedDate)
    {
        var result = await _issuedBookService.ExtendBookSubmissionDate(issueId, extendedDate);
        return StatusCode(result.StatusCode, result);
    }
}