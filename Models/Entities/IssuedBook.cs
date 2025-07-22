using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lmsApi.Models.Entities;
public class IssuedBook
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IssueId { get; set; }


    [Required(ErrorMessage = "User ID is required")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Copy ID is required")]
    public int CopyId { get; set; }

    [Required(ErrorMessage = "Issued by is required")]
    public Guid IssuedBy { get; set; } 

    [Required]
    public IssueStatus Status { get; set; } = IssueStatus.Issued;

    [Required(ErrorMessage = "Issue date is required")]
    [Column(TypeName = "DATETIME")]
    public DateTime IssuedDate { get; set; } = DateTime.UtcNow;

    [Required(ErrorMessage = "Due date is required")]
    [Column(TypeName = "DATETIME")]
    public DateTime DueDate { get; set; }

    [Column(TypeName = "DATETIME")]
    public DateTime? ReturnDate { get; set; }

    [Column(TypeName = "DATETIME")]
    public DateTime? ExtendedDate { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Fine amount cannot be negative")]
    [Column(TypeName = "DECIMAL(10,2)")]
    public decimal Fine { get; set; } = 0;

    [Range(0, double.MaxValue, ErrorMessage = "Fine paid cannot be negative")]
    [Column(TypeName = "DECIMAL(10,2)")]
    public decimal FinePaid { get; set; } = 0;

    [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
    [Column(TypeName = "VARCHAR(500)")]
    public string? Notes { get; set; }

    public bool IsExtended { get; set; } = false;

    [Range(0, 3, ErrorMessage = "Extension count cannot exceed 3")]
    public int ExtensionCount { get; set; } = 0;

    [Column(TypeName = "DATETIME")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "DATETIME")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
    
    [ForeignKey("CopyId")]
    public virtual BookCopy BookCopy { get; set; } = null!;
    
    [ForeignKey("IssuedBy")]
    public virtual User IssuedByUser { get; set; } = null!;

 
    [NotMapped]
    public bool IsOverdue => DateTime.UtcNow > DueDate && ReturnDate == null;

    [NotMapped]
    public int DaysOverdue => IsOverdue ? (DateTime.UtcNow - DueDate).Days : 0;

    [NotMapped]
    public decimal OutstandingFine => Fine - FinePaid;

    [NotMapped]
    public bool HasOutstandingFine => OutstandingFine > 0;

    [NotMapped]
    public string StatusDisplay => Status switch
    {
        IssueStatus.Issued => IsOverdue ? "Overdue" : "Issued",
        IssueStatus.Returned => "Returned",
        IssueStatus.Overdue => "Overdue",
        IssueStatus.Lost => "Lost",
        IssueStatus.Damaged => "Damaged",
        _ => "Unknown"
    };

    [NotMapped]
    public int IssueDurationDays => ReturnDate.HasValue 
        ? (ReturnDate.Value - IssuedDate).Days 
        : (DateTime.UtcNow - IssuedDate).Days;

    [NotMapped]
    public bool CanExtend => !IsOverdue && ExtensionCount < 3 && ReturnDate == null;
}
