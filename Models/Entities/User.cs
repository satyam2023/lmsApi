using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(100, MinimumLength = 2)]
    [Column(TypeName = "VARCHAR(100)")]
    public string Name { get; set; } = "";

    [Required]
    [EmailAddress]
    [StringLength(255)]
    [Column(TypeName = "VARCHAR(255)")]
    public string Email { get; set; } = "";

    [Phone]
    [StringLength(15)]
    [Column(TypeName = "VARCHAR(15)")]
    public string PhoneNumber { get; set; } = "";

    [Required]
    [StringLength(255, MinimumLength = 6)]
    [Column(TypeName = "VARCHAR(255)")]
    public string Password { get; set; } = "";

    [Required]
    [StringLength(20)]
    [Column(TypeName = "VARCHAR(20)")]
    public string Role { get; set; } = "";

    [StringLength(500)]
    [Column(TypeName = "VARCHAR(500)")]
    public string RefreshToken { get; set; } = "";

    [StringLength(500)]
    [Column(TypeName = "VARCHAR(500)")]
    public string AccessToken { get; set; } = "";

    [Column(TypeName = "DATETIME")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "DATETIME")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}