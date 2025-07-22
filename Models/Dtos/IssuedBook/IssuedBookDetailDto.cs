using System;
using lmsApi.Models.Entities;

namespace lmsApi.Models.Dtos.IssuedBook;

public class IssuedBookDetailDto
{
    public int IssueId { get; set; }
    public Guid UserId { get; set; }
    public int CopyId { get; set; }
    public Guid IssuedBy { get; set; }
    public IssueStatus Status { get; set; }
    public DateTime IssuedDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public decimal Fine { get; set; }
    public decimal FinePaid { get; set; }
    public string? Notes { get; set; }
    public bool IsExtended { get; set; }
    public int ExtensionCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string StatusDisplay { get; set; } = string.Empty;
    public bool IsOverdue { get; set; }
    public int DaysOverdue { get; set; }
    public decimal OutstandingFine { get; set; }
    public bool HasOutstandingFine { get; set; }
}
