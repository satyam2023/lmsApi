using System.ComponentModel.DataAnnotations;

namespace lmsApi.Models.Dtos.User;

public record LoginUserDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
    public string Email { get; init; } = "";

    [Required(ErrorMessage = "Password is required")]
    [StringLength(255, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; init; } = "";
}
