using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace lmsApi.Models.Dtos.IssuedBook;

public class IssueBookDto
{
    [Required(ErrorMessage = "User ID is required")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Copy ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Copy ID must be greater than 0")]
    public int CopyId { get; set; }

    [Required(ErrorMessage = "Due date is required")]
    [DataType(DataType.DateTime)]
    public DateTime DueDate { get; set; }

    [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
    public string? Notes { get; set; }
}
