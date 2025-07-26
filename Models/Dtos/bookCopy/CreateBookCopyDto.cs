using System.ComponentModel.DataAnnotations;

namespace lmsApi.Models.Dtos.BookCopy;

public record CreateBookCopy
{
    [Required(ErrorMessage = "Book ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Book ID must be a valid positive number")]
    public int BookId { get; set; }

    public bool IsAvailable { get; set; } = true;
    public bool IsActive { get; set; } = true;
}
