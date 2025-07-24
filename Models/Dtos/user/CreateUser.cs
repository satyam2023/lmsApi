using System.ComponentModel.DataAnnotations;

namespace lmsApi.Models.Dtos.User;
public record CreateUser
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    public string Name { get; init; } = "";

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
    public string Email { get; init; } = "";

    [Phone(ErrorMessage = "Please enter a valid phone number")]
    [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters")]
    public string PhoneNumber { get; init; } = "";

    [Required(ErrorMessage = "Password is required")]
    [StringLength(255, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; init; } = "";

    [Required(ErrorMessage = "Confirm Password is required")]
    [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
    public string ConfirmPassword { get; init; } = "";

    [Required(ErrorMessage = "Role is required")]
    public Role Role { get; init; } = Role.User;
}