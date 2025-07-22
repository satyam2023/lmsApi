using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lmsApi.Models.Entities;

public class BookCopy
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CopyId { get; set; }

    [Required(ErrorMessage = "Book ID is required")]
    [ForeignKey("Book")]
    public int BookId { get; set; }

    [Required(ErrorMessage = "Availability status is required")]
    public bool IsAvailable { get; set; } = true;

    [Column(TypeName = "DATETIME")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "DATETIME")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

 
    public virtual Book Book { get; set; } = null!;
    public virtual ICollection<IssuedBook> IssuedBooks { get; set; } = new List<IssuedBook>();

 
    [NotMapped]
    public bool IsCurrentlyIssued => IssuedBooks?.Any(ib => ib.ReturnDate == null) ?? false;

    [NotMapped]
    public IssuedBook? CurrentIssue => IssuedBooks?.FirstOrDefault(ib => ib.ReturnDate == null);

    [NotMapped]
    public string CopyDisplayName => $"{Book?.Title} - Copy #{CopyId}";
}
