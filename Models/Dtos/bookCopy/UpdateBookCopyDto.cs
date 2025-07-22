using lmsApi.Attributes;
using System.ComponentModel.DataAnnotations;

namespace lmsApi.Models.Dtos.BookCopy;

[AtLeastOneProperty(ErrorMessage = "At least one field must be provided for update")]
public record UpdateBookCopyDto
{
    public bool? IsAvailable { get; set; }
    public bool? IsActive { get; set; }
}
