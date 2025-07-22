using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lmsApi.Models.Entities;

public class BookCategory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    [Column(TypeName = "VARCHAR(100)")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 500 characters")]
    [Column(TypeName = "VARCHAR(500)")]
    public string? Description { get; set; }

    [StringLength(255, ErrorMessage = "Image URL cannot exceed 255 characters")]
    [Column(TypeName = "VARCHAR(255)")]
    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; } = true;

    [Column(TypeName = "DATETIME")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "DATETIME")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();


    [NotMapped]
    public int BookCount => Books?.Count ?? 0;

    [NotMapped]
    public int ActiveBookCount => Books?.Count(b => b.IsActive) ?? 0;
}
