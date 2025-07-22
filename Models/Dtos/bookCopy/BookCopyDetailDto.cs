namespace lmsApi.Models.Dtos.BookCopy;

public record BookCopyDetailDto
{
    public int CopyId { get; init; }
    public int BookId { get; init; }
    public string BookTitle { get; init; } = string.Empty;
    public string BookAuthor { get; init; } = string.Empty;
    public bool IsAvailable { get; init; }
    public bool IsCurrentlyIssued { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public bool IsActive { get; init; }
}
