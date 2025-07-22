using AutoMapper;
using ECommerceApp.ApiResponse;
using lmsApi.Constants.AppStatusCode;
using lmsApi.Data;
using lmsApi.Models.Dtos.User;
using Microsoft.EntityFrameworkCore;
using lmsApi.Helper.Jwt;

public interface IUserService
{
    Task<ApiResponse<UserDetail>> CreateUser(CreateUserDto userDetail);
    Task<ApiResponse<UserDetail>> LoginUser(LoginUserDto loginUser);
    Task<ApiResponse<UserDetail>> UpdateUser(UpdateUserDto user, Guid id);
    Task<ApiResponse<RefreshTokenResponse>> RefreshToken(RefreshTokenRequest refreshTokenDto);
}

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly JwtHelper _jwtHelper;

    public UserService(ApplicationDbContext context, IMapper mapper, JwtHelper jwtHelper)
    {
        _context = context;
        _mapper = mapper;
        _jwtHelper = jwtHelper;
    }

    public async Task<ApiResponse<UserDetail>> CreateUser(CreateUserDto userDetail)
    {
        var isExist = await _context.Users.AnyAsync(u => u.Email == userDetail.Email);
        if (isExist)
        {
            return new ApiResponse<UserDetail>
            {
                Message = "User with this email already exists",
                StatusCode = AppStatusCode.AlreadyExists
            };
        }

        var userToAdd = _mapper.Map<User>(userDetail);


        userToAdd.Password = BCrypt.Net.BCrypt.HashPassword(userDetail.Password);


        userToAdd.AccessToken = _jwtHelper.GenerateAccessToken(userToAdd);
        userToAdd.RefreshToken = _jwtHelper.GenerateRefreshToken();


        userToAdd.CreatedAt = DateTime.UtcNow;
        userToAdd.UpdatedAt = DateTime.UtcNow;

        await _context.Users.AddAsync(userToAdd);
        await _context.SaveChangesAsync();

        var result = _mapper.Map<UserDetail>(userToAdd);
        return new ApiResponse<UserDetail>
        {
            Data = result,
            Message = "User created successfully",
            StatusCode = AppStatusCode.Created
        };
    }

    public async Task<ApiResponse<UserDetail>> LoginUser(LoginUserDto loginUser)
    {

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginUser.Email);
        if (user == null)
        {
            return new ApiResponse<UserDetail>
            {
                Message = "Invalid email or password",
                StatusCode = AppStatusCode.NotFound
            };
        }


        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginUser.Password, user.Password);
        if (!isPasswordValid)
        {
            return new ApiResponse<UserDetail>
            {
                Message = "Invalid email or password",
                StatusCode = AppStatusCode.Unauthorized
            };
        }


        user.AccessToken = _jwtHelper.GenerateAccessToken(user);
        user.RefreshToken = _jwtHelper.GenerateRefreshToken();
        user.UpdatedAt = DateTime.UtcNow;


        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        var result = _mapper.Map<UserDetail>(user);
        return new ApiResponse<UserDetail>
        {
            Data = result,
            Message = "Login successful",
            StatusCode = AppStatusCode.Success
        };
    }
    
    public async Task<ApiResponse<UserDetail>> UpdateUser(UpdateUserDto user, Guid id)
    {
        Console.WriteLine($"Updating user with ID: {id}");
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (existingUser == null)
        {
            return new ApiResponse<UserDetail>
            {
                Message = "User not found",
                StatusCode = AppStatusCode.NotFound
            };
        }

        // Only update fields that are provided (not null)
        if (!string.IsNullOrWhiteSpace(user.Name))
        {
            existingUser.Name = user.Name;
        }

        if (!string.IsNullOrWhiteSpace(user.Email))
        {
            // Check if the new email is already taken by another user
            var emailExists = await _context.Users.AnyAsync(u => u.Email == user.Email && u.Id != id);
            if (emailExists)
            {
                return new ApiResponse<UserDetail>
                {
                    Message = "Email is already taken by another user",
                    StatusCode = AppStatusCode.BadRequest
                };
            }
            existingUser.Email = user.Email;
        }

        if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
        {
            existingUser.PhoneNumber = user.PhoneNumber;
        }

        existingUser.UpdatedAt = DateTime.UtcNow;

        _context.Users.Update(existingUser);
        await _context.SaveChangesAsync();

        var result = _mapper.Map<UserDetail>(existingUser);
        return new ApiResponse<UserDetail>
        {
            Data = result,
            Message = "User updated successfully",
            StatusCode = AppStatusCode.Success
        };
    }

    public async Task<ApiResponse<RefreshTokenResponse>> RefreshToken(RefreshTokenRequest refreshTokenDto)
    {

        var user =await _context.Users.FirstAsync(u => u.RefreshToken == refreshTokenDto.RefreshToken);

        if (user == null)
        {
            return new ApiResponse<RefreshTokenResponse>
            {
                Message = "Invalid access token",
                StatusCode = AppStatusCode.Unauthorized
            };
        }

        var generateNewAccessToken = _jwtHelper.GenerateAccessToken(user);

        var result = new RefreshTokenResponse
        {
            AccessToken = generateNewAccessToken,
            RefreshToken = user.RefreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(1) 
        };
        return new ApiResponse<RefreshTokenResponse>
        {
            Data = result,
            Message = "Token refreshed successfully",
            StatusCode = AppStatusCode.Success
        };
    }
}