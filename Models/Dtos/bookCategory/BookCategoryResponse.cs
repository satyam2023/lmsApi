
using System.ComponentModel.DataAnnotations;
using lmsApi.Models.Entities;

public record BookDetailForCategory {
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string? ISBN { get; set; }
    public string? Publisher { get; set; }
    public string? ImageUrl { get; set; }
    [Required(ErrorMessage = "Total copies is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Total copies must be at least 1")]
    public int TotalCopies { get; set; }
    public bool IsActive { get; set; } = true;
    public int AvailableCopies { get; set; }
    public int IssuedCopies { get; set; }
    public bool HasAvailableCopies { get; set; }
}

public record BookCategoryResponse
{
    public int CategoryId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string? ImageUrl { get; init; }
    public List<BookDetailForCategory> Books { get; init; } = new List<BookDetailForCategory>();
    public int BookCount { get; init; }
}