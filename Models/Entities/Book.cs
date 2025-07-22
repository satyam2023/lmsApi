using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lmsApi.Models.Entities;

public class Book
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BookId { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters")]
    [Column(TypeName = "VARCHAR(200)")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Author is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Author must be between 2 and 100 characters")]
    [Column(TypeName = "VARCHAR(100)")]
    public string Author { get; set; } = string.Empty;

    [StringLength(20, MinimumLength = 10, ErrorMessage = "ISBN must be between 10 and 20 characters")]
    [Column(TypeName = "VARCHAR(20)")]
    public string? ISBN { get; set; }

    [StringLength(100, ErrorMessage = "Publisher name cannot exceed 100 characters")]
    [Column(TypeName = "VARCHAR(100)")]
    public string? Publisher { get; set; }

    [StringLength(255, ErrorMessage = "Image URL cannot exceed 255 characters")]
    [Column(TypeName = "VARCHAR(255)")]
    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "Total copies is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Total copies must be at least 1")]
    public int TotalCopies { get; set; }

    [Column(TypeName = "DATETIME")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "DATETIME")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;


    [Required(ErrorMessage = "Category is required")]
    [ForeignKey("Category")]
    public int CategoryId { get; set; }

  
    public virtual BookCategory Category { get; set; } = null!;
    public virtual ICollection<BookCopy> BookCopies { get; set; } = new List<BookCopy>();

    [NotMapped]
    public int AvailableCopies => BookCopies?.Count(bc => bc.IsAvailable) ?? 0;

    [NotMapped]
    public int IssuedCopies => TotalCopies - AvailableCopies;

    [NotMapped]
    public bool HasAvailableCopies => AvailableCopies > 0;
}
