

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

}