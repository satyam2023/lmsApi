using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lmsApi.Models.Entities;

public class CreateBookDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Author is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Author must be between 2 and 100 characters")]
    public string Author { get; set; } = string.Empty;

    [StringLength(20, MinimumLength = 10, ErrorMessage = "ISBN must be between 10 and 20 characters")]
    public string? ISBN { get; set; }

    [StringLength(100, ErrorMessage = "Publisher name cannot exceed 100 characters")]
    public string? Publisher { get; set; }

    [StringLength(255, ErrorMessage = "Image URL cannot exceed 255 characters")]

    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;

    [Required(ErrorMessage = "Category is required")]
    [ForeignKey("Category")]
    public int CategoryId { get; set; }

}
