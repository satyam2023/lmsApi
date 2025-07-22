
using System.ComponentModel.DataAnnotations;
using lmsApi.Attributes;

[AtLeastOneProperty]
public record UpdateUserDto
{
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    public string? Name { get; init; }

    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
    public string? Email { get; init; }

    [Phone(ErrorMessage = "Please enter a valid phone number")]
    [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters")]
    public string? PhoneNumber { get; init; }
}