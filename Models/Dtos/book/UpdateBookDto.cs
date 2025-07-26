using System.ComponentModel.DataAnnotations;
using lmsApi.Attributes;

[AtLeastOneProperty] 
public class UpdateBookRequest
{
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters.")]
    public string? Title { get; set; }

    [StringLength(100, MinimumLength = 2, ErrorMessage = "Author must be between 2 and 100 characters.")]
    public string? Author { get; set; }

    [StringLength(100, ErrorMessage = "Publisher name cannot exceed 100 characters.")]
    public string? Publisher { get; set; }

    [StringLength(255, ErrorMessage = "Image URL cannot exceed 255 characters.")]
    [Url(ErrorMessage = "ImageUrl must be a valid URL.")]
    public string? ImageUrl { get; set; }
}
