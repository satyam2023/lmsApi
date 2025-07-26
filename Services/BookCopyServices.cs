
using AutoMapper;
using ECommerceApp.ApiResponse;
using lmsApi.Constants.AppStatusCode;
using lmsApi.Data;
using lmsApi.Models.Entities;
using lmsApi.Models.Dtos.BookCopy;
using Microsoft.EntityFrameworkCore;

namespace lmsApi.Services;

public interface IBookCopyServices
{
    Task<ApiResponse<string>> CreateBookCopy(CreateBookCopy createBookCopy);
    Task<ApiResponse<List<BookCopyDetailDto>>> GetBookCopiesByBookId(int bookId);
    Task<ApiResponse<BookCopyDetailDto>> UpdateBookCopy(int id, UpdateBookCopyDto updateBookCopy);
    Task<ApiResponse<bool>> DeleteBookCopy(int id);
}

public class BookCopyServices : IBookCopyServices
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public BookCopyServices(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<string>> CreateBookCopy(CreateBookCopy createBookCopy)
    {

        var book = await _context.Books.FindAsync(createBookCopy.BookId);
        if (book == null)
        {
            return new ApiResponse<string>
            {
                StatusCode = AppStatusCode.NotFound,
                Message = "Book not found",
                Errors = new List<string> { "Invalid book ID" }
            };
        }


        if (!book.IsActive)
        {
            return new ApiResponse<string>
            {
                StatusCode = AppStatusCode.BadRequest,
                Message = "Cannot add copy to an inactive book",
                Errors = new List<string> { "Book is not active" }
            };
        }


        BookCopy bookCopy = _mapper.Map<BookCopy>(createBookCopy);

        book.TotalCopies += 1;
        await _context.BookCopies.AddAsync(bookCopy);
        await _context.SaveChangesAsync();


        return new ApiResponse<string>
        {
            StatusCode = AppStatusCode.Created,
            Data = $"Your Book copy of {book.Title} has been added successfuly and the copy id is {bookCopy.CopyId}",
            Message = "Book copy created successfully"
        };
    }
    public async Task<ApiResponse<List<BookCopyDetailDto>>> GetBookCopiesByBookId(int bookId)
    {
        var bookCopies = await _context.BookCopies
            .Where(bc => bc.BookId == bookId && bc.IsActive && bc.Book.IsActive)
            .OrderBy(bc => bc.CopyId)
            .Select(bc => new BookCopyDetailDto
            {
                CopyId = bc.CopyId,
                BookId = bc.BookId,
                BookTitle = bc.Book.Title,
                BookAuthor = bc.Book.Author,
                IsAvailable = bc.IsAvailable,
                IsCurrentlyIssued = !bc.IsAvailable,
                CreatedAt = bc.CreatedAt,
                UpdatedAt = bc.UpdatedAt,
                IsActive = bc.IsActive
            })
            .ToListAsync();

        if (bookCopies.Count == 0)
        {
            return new ApiResponse<List<BookCopyDetailDto>>
            {
                StatusCode = AppStatusCode.NotFound,
                Message = "Book not found or has no active copies"
            };
        }

        return new ApiResponse<List<BookCopyDetailDto>>
        {
            StatusCode = AppStatusCode.Success,
            Data = bookCopies,
            Message = $"Found {bookCopies.Count} copies for the book"
        };
    }

    public async Task<ApiResponse<BookCopyDetailDto>> UpdateBookCopy(int id, UpdateBookCopyDto updateBookCopy)
    {

        var bookCopy = await _context.BookCopies.FindAsync(id);

        if (bookCopy == null)
        {
            return new ApiResponse<BookCopyDetailDto>
            {
                StatusCode = AppStatusCode.NotFound,
                Message = "Book copy not found"
            };
        }


        if (updateBookCopy.IsAvailable.HasValue)
        {
            bookCopy.IsAvailable = updateBookCopy.IsAvailable.Value;
        }

        if (updateBookCopy.IsActive.HasValue)
        {
            bookCopy.IsActive = updateBookCopy.IsActive.Value;
        }

        bookCopy.UpdatedAt = DateTime.UtcNow;

        _context.BookCopies.Update(bookCopy);
        await _context.SaveChangesAsync();

        BookCopyDetailDto response = _mapper.Map<BookCopyDetailDto>(bookCopy);
        return new ApiResponse<BookCopyDetailDto>
        {
            StatusCode = AppStatusCode.Success,
            Data = response,
            Message = "Book copy updated successfully"
        };

    }

    public async Task<ApiResponse<bool>> DeleteBookCopy(int id)
    {

        var bookCopy = await _context.BookCopies.FindAsync(id);

        if (bookCopy == null || !bookCopy.IsActive)
        {
            return new ApiResponse<bool>
            {
                StatusCode = AppStatusCode.NotFound,
                Message = "Book copy not found"
            };
        }


        bookCopy.IsActive = false;
        bookCopy.IsAvailable = false;
        bookCopy.UpdatedAt = DateTime.UtcNow;

        _context.BookCopies.Update(bookCopy);
        await _context.SaveChangesAsync();

        return new ApiResponse<bool>
        {
            StatusCode = AppStatusCode.Success,
            Data = true,
            Message = "Book copy deleted successfully"
        };

    }


}