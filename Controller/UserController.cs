using ECommerceApp.ApiResponse;
using lmsApi.Data;
using lmsApi.Models.Dtos.User;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IUserService _userService;
    public UserController(ApplicationDbContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto user)
    {

        ApiResponse<UserDetail> response = await _userService.CreateUser(user);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpPost("logIn")]
    public async Task<IActionResult> LogInUser([FromBody] LoginUserDto loginUser)
    {

        ApiResponse<UserDetail> response = await _userService.LoginUser(loginUser);
        return StatusCode((int)response.StatusCode, response);
    }
    
    [HttpPut("updateUser/{id}")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto user,[FromRoute] Guid id)
    {

        ApiResponse<UserDetail> response = await _userService.UpdateUser(user,id);
        return StatusCode((int)response.StatusCode, response);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenDto)
    {
        ApiResponse<RefreshTokenResponse> response = await _userService.RefreshToken(refreshTokenDto);
        return StatusCode((int)response.StatusCode, response);
    }

}