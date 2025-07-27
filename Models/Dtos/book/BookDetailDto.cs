

public record BookDetailDto
{
    public int BookId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Author { get; init; } = string.Empty;
    public string? ISBN { get; init; }
    public string? Publisher { get; init; }
    public string? ImageUrl { get; init; }
    public int TotalCopies { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
    public bool IsActive { get; init; } = true;
}