using System.ComponentModel.DataAnnotations;

namespace lmsApi.Models.Dtos.User;

public record UserDetail
{
    public Guid Id { get; init; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; init; } = "";
    
    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; init; } = "";
    
    [Phone]
    [StringLength(15)]
    public string PhoneNumber { get; init; } = "";
    
    [Required]
    [StringLength(20)]
    public string Role { get; init; } = "";
    
    [StringLength(500)]
    public string AccessToken { get; init; } = "";
    
    [StringLength(500)]
    public string RefreshToken { get; init; } = "";
    
    public DateTime CreatedAt { get; init; }
    
    public DateTime UpdatedAt { get; init; }
}