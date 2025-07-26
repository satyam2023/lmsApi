using System;
using System.ComponentModel.DataAnnotations;

namespace lmsApi.Models.Dtos.IssuedBook;

public class SubmitBook
{
    [Required(ErrorMessage = "Issue ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Issue ID must be greater than 0")]
    public int IssueId { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Fine paid cannot be negative")]
    public decimal FinePaid { get; set; } = 0;

    [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
    public string? Notes { get; set; }
}
