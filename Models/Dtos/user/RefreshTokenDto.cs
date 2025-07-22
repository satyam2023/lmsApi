using System.ComponentModel.DataAnnotations;

namespace lmsApi.Models.Dtos.User;

public class RefreshTokenRequest
{
    [Required(ErrorMessage = "Refresh token is required")]
    public string RefreshToken { get; set; } = "";
}

public record RefreshTokenResponse
{
    public string AccessToken { get; init; } = "";
    public string RefreshToken { get; init; } = "";
    public DateTime ExpiresAt { get; init; } = DateTime.UtcNow.AddDays(1);
}

