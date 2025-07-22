using System.ComponentModel.DataAnnotations;
using lmsApi.Attributes;

namespace lmsApi.Models.Dtos.BookCategory;

[AtLeastOneProperty(ErrorMessage = "At least one field must be provided for update")]
public record UpdateBookCategoryDto
{
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    public string? Name { get; set; }

    [StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 500 characters")]
    public string? Description { get; set; }

    [StringLength(255, ErrorMessage = "Image URL cannot exceed 255 characters")]
    public string? ImageUrl { get; set; }

    public bool? IsActive { get; set; }
}
