
public record IssuedBookDetailToUser
{
    public int BookId { get; init; }
    public int CopyId { get; init; }
    public Guid IssuedBy { get; init; }
    public string BookTitle { get; init; } = string.Empty;
    public string AuthorName { get; init; } = string.Empty;
    public DateTime IssueDate { get; init; }
    public DateTime DueDate { get; init; }
    public DateTime? ReturnDate { get; init; }
    public IssueStatus Status { get; init; }
    public int IssueId { get; init; }
    public decimal? Fine =>
    DateTime.Now.Date > DueDate.Date && !ReturnDate.HasValue
        ? (decimal)(DateTime.Now.Date - DueDate.Date).TotalDays * 4
        : 0;
}