using lmsApi.Data;
using lmsApi.Models.Entities;
using lmsApi.Models.Dtos.IssuedBook;
using lmsApi.Constants.AppStatusCode;
using ECommerceApp.ApiResponse;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using lmsApi.Helper.Jwt;

namespace lmsApi.Services;

public interface IIssuedBookService
{
    Task<ApiResponse<string>> IssueBook(IssueBook dto, string token);
    Task<ApiResponse<string>> SubmitBook(SubmitBook dto);

    Task<ApiResponse<List<IssuedBookDetailToUser>>> GetIssuedBooksToUser(Guid userId);
}

public class IssuedBookService : IIssuedBookService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public IssuedBookService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<string>> IssueBook(IssueBook dto, string token)
    {
        var bookCopy = await _context.BookCopies.FirstOrDefaultAsync(bc => bc.CopyId == dto.CopyId && bc.IsAvailable && bc.IsActive);
        if (bookCopy == null)
        {
            return new ApiResponse<string>
            {
                StatusCode = AppStatusCode.NotFound,
                Message = "Book copy not available for issue",
                Errors = new List<string> { "Invalid or unavailable copy ID" }
            };
        }

        var alreadyIssued = await _context.IssuedBooks.AnyAsync(ib => ib.CopyId == dto.CopyId && ib.Status == IssueStatus.Issued);
        if (alreadyIssued)
        {
            return new ApiResponse<string>
            {
                StatusCode = AppStatusCode.BadRequest,
                Message = "Book copy is already issued",
                Errors = new List<string> { "Copy is already issued" }
            };
        }

        Guid issuerId = JwtHelper.ExtractUserIdFromToken(token);
        Console.WriteLine($"Processing issue for CopyId: {dto.CopyId}, IssuerId: {issuerId}");
        IssuedBook issuedBook = _mapper.Map<IssuedBook>(dto);
        issuedBook.IssuedBy = issuerId;

        bookCopy.IsAvailable = false;
        bookCopy.UpdatedAt = DateTime.UtcNow;

        await _context.IssuedBooks.AddAsync(issuedBook);
        _context.BookCopies.Update(bookCopy);
        await _context.SaveChangesAsync();
        return new ApiResponse<string>
        {
            StatusCode = AppStatusCode.Created,
            Data = $"Your Book has been issued successfully and due date is {issuedBook.DueDate}",
            Message = "Book issued successfully"
        };
    }

    public async Task<ApiResponse<string>> SubmitBook(SubmitBook dto)
    {
        var issuedBook = await _context.IssuedBooks
            .Include(ib => ib.BookCopy)
            .FirstOrDefaultAsync(ib => ib.IssueId == dto.IssueId);

        if (issuedBook == null)
        {
            return new ApiResponse<string>
            {
                StatusCode = AppStatusCode.NotFound,
                Message = "Issued book record not found",
                Errors = new List<string> { "Invalid issue ID" }
            };
        }

        if (issuedBook.Status != IssueStatus.Issued)
        {
            return new ApiResponse<string>
            {
                StatusCode = AppStatusCode.BadRequest,
                Message = "Book is not currently issued",
                Errors = new List<string> { "Book is not in issued state" }
            };
        }

        if (issuedBook.Fine <= 0 && issuedBook.DaysOverdue > 0)
        {
            issuedBook.Fine = issuedBook.DaysOverdue * 4;
        }

        var totalFineToBePaid = issuedBook.Fine;
        var totalPaidIncludingThis = issuedBook.FinePaid + dto.FinePaid;

        if (totalFineToBePaid > 0 && totalPaidIncludingThis < totalFineToBePaid)
        {
            return new ApiResponse<string>
            {
                StatusCode = AppStatusCode.BadRequest,
                Message = "Fine not fully paid",
                Errors = new List<string> {
                $"Outstanding fine is {totalFineToBePaid - issuedBook.FinePaid}, " +
                $"but only {dto.FinePaid} paid in this transaction"
            }
            };
        }


        issuedBook.Status = IssueStatus.Returned;
        issuedBook.ReturnDate = DateTime.UtcNow;
        issuedBook.UpdatedAt = DateTime.UtcNow;
        issuedBook.FinePaid += dto.FinePaid;

        if (!string.IsNullOrWhiteSpace(dto.Notes))
        {
            issuedBook.Notes = dto.Notes;
        }

        issuedBook.BookCopy.IsAvailable = true;
        issuedBook.BookCopy.UpdatedAt = DateTime.UtcNow;

        _context.IssuedBooks.Update(issuedBook);
        _context.BookCopies.Update(issuedBook.BookCopy);
        await _context.SaveChangesAsync();

        return new ApiResponse<string>
        {
            StatusCode = AppStatusCode.Success,
            Data = $"Your book has been submitted successfully. ",
            Message = "Book submitted successfully"
        };
    }

    public async Task<ApiResponse<List<IssuedBookDetailToUser>>> GetIssuedBooksToUser(Guid userId)
    {
        var issuedBooks = await _context.IssuedBooks
    .Where(ib => ib.UserId == userId)
    .Include(ib => ib.BookCopy)
        .ThenInclude(bc => bc.Book)
    .Select(ib => new IssuedBookDetailToUser
    {
        BookId = ib.BookCopy.BookId,
        CopyId = ib.CopyId,
        IssuedBy = ib.UserId,
        BookTitle = ib.BookCopy.Book.Title,
        AuthorName = ib.BookCopy.Book.Author,
        IssueDate = ib.IssuedDate,
        DueDate = ib.DueDate,
        ReturnDate = ib.ReturnDate,
        Status = ib.Status,
        IssueId = ib.IssueId,
    })
    .ToListAsync();
        if (issuedBooks.Count == 0)
        {
            return new ApiResponse<List<IssuedBookDetailToUser>>
            {
                StatusCode = AppStatusCode.NotFound,
                Message = "No issued books found for this user"
            };
        }
        return new ApiResponse<List<IssuedBookDetailToUser>>
        {
            StatusCode = AppStatusCode.Success,
            Data = issuedBooks,
            Message = "Issued books retrieved successfully"
        };
    }

}
