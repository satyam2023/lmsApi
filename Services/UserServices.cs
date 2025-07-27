using AutoMapper;
using lmsApi.Constants.AppStatusCode;
using lmsApi.Data;
using lmsApi.Models.Dtos.User;
using Microsoft.EntityFrameworkCore;
using lmsApi.Helper.Jwt;
using lmsApi.ApiResponse;

public interface IUserService
{
    Task<ApiResponse<UserDetail>> CreateUser(CreateUser userDetail);
    Task<ApiResponse<UserDetail>> LoginUser(LoginUser loginUser);
    Task<ApiResponse<UserDetail>> UpdateUser(UpdateUser user, Guid id);
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

    public async Task<ApiResponse<UserDetail>> CreateUser(CreateUser userDetail)
    {
        var isExist = await _context.Users.AnyAsync(u => u.Email == userDetail.Email);
        if (isExist)
        {
            return new ApiResponse<UserDetail>
            {
                StatusCode = AppStatusCode.AlreadyExists,
                Error = "User with this email already exists"
            };
        }

        var userToAdd = _mapper.Map<User>(userDetail);
        userToAdd.Password = BCrypt.Net.BCrypt.HashPassword(userDetail.Password);
        userToAdd.RefreshToken = _jwtHelper.GenerateRefreshToken();
        userToAdd.CreatedAt = DateTime.UtcNow;
        userToAdd.UpdatedAt = DateTime.UtcNow;

        var accessToken = _jwtHelper.GenerateAccessToken(userToAdd);

        await _context.Users.AddAsync(userToAdd);
        await _context.SaveChangesAsync();

        var result = _mapper.Map<UserDetail>(userToAdd) with { AccessToken = accessToken };

        return new ApiResponse<UserDetail>
        {
            StatusCode = AppStatusCode.Created,
            Data = result,
            Message = "User created successfully"
        };
    }

    public async Task<ApiResponse<UserDetail>> LoginUser(LoginUser loginUser)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginUser.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginUser.Password, user.Password))
        {
            return new ApiResponse<UserDetail>
            {
                StatusCode = AppStatusCode.Unauthorized,
                Error = "Invalid email or password"
            };
        }

        user.RefreshToken = _jwtHelper.GenerateRefreshToken();
        user.UpdatedAt = DateTime.UtcNow;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        var accessToken = _jwtHelper.GenerateAccessToken(user);
        var result = _mapper.Map<UserDetail>(user) with { AccessToken = accessToken };

        return new ApiResponse<UserDetail>
        {
            StatusCode = AppStatusCode.Success,
            Data = result,
            Message = "Login successful"
        };
    }

    public async Task<ApiResponse<UserDetail>> UpdateUser(UpdateUser user, Guid id)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (existingUser == null)
        {
            return new ApiResponse<UserDetail>
            {
                StatusCode = AppStatusCode.NotFound,
                Error = "User not found"
            };
        }

        if (!string.IsNullOrWhiteSpace(user.Name))
        {
            existingUser.Name = user.Name;
        }

        if (!string.IsNullOrWhiteSpace(user.Email))
        {
            var emailExists = await _context.Users.AnyAsync(u => u.Email == user.Email && u.Id != id);
            if (emailExists)
            {
                return new ApiResponse<UserDetail>
                {
                    StatusCode = AppStatusCode.BadRequest,
                    Error = "Email is already taken by another user"
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

        var accessToken = _jwtHelper.GenerateAccessToken(existingUser);
        var result = _mapper.Map<UserDetail>(existingUser) with { AccessToken = accessToken };

        return new ApiResponse<UserDetail>
        {
            StatusCode = AppStatusCode.Success,
            Data = result,
            Message = "User updated successfully"
        };
    }

    public async Task<ApiResponse<RefreshTokenResponse>> RefreshToken(RefreshTokenRequest refreshTokenDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshTokenDto.RefreshToken);
        if (user == null)
        {
            return new ApiResponse<RefreshTokenResponse>
            {
                StatusCode = AppStatusCode.Unauthorized,
                Error = "Invalid refresh token"
            };
        }

        var newAccessToken = _jwtHelper.GenerateAccessToken(user);

        var result = new RefreshTokenResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = user.RefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(1)
        };

        return new ApiResponse<RefreshTokenResponse>
        {
            StatusCode = AppStatusCode.Success,
            Data = result,
            Message = "Token refreshed successfully"
        };
    }
}
