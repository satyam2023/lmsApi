
using AutoMapper;
using ECommerceApp.ApiResponse;
using lmsApi.Constants.AppStatusCode;
using lmsApi.Data;
using lmsApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

public interface IBookServices
{
   Task<ApiResponse<BookDetailDto>> CreateBook(CreateBook book);
   Task<ApiResponse<BookDetailDto>> UpdateBookDetail(UpdateBookRequest updateBookDto,int bookId);
}

public class BookServices : IBookServices
{
        private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public BookServices(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
  
    public async Task<ApiResponse<BookDetailDto>> CreateBook(CreateBook book)
    {
        var existingBook = await _context.Books
            .FirstOrDefaultAsync(b => b.Title.ToLower() == book.Title.ToLower() && b.Author.ToLower() == book.Author.ToLower());

        if (existingBook != null)
        {
            return new ApiResponse<BookDetailDto>
            {
                StatusCode = AppStatusCode.BadRequest,
                Message = "Book with this title and author already exists",
                Errors = new List<string> { "Duplicate book entry" }
            };
        }

    
        var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryId == book.CategoryId);
        if (!categoryExists)
        {
            return new ApiResponse<BookDetailDto>
            {
                StatusCode = AppStatusCode.BadRequest,
                Message = "Category not found",
                Errors = new List<string> { "Invalid category ID" }
            };
        }

    
        Book bookToAdd = _mapper.Map<Book>(book);
        bookToAdd.TotalCopies = 0;

        await _context.Books.AddAsync(bookToAdd);
        await _context.SaveChangesAsync();
        
        BookDetailDto bookResponse = _mapper.Map<BookDetailDto>(bookToAdd);

        return new ApiResponse<BookDetailDto>
        {
            StatusCode = AppStatusCode.Created,
            Data = bookResponse,
            Message = "Book created successfully"
        };
    }
    public async Task<ApiResponse<BookDetailDto>> UpdateBookDetail(UpdateBookRequest updateBookDto,int bookId)
    {
        var book = await _context.Books.FindAsync(bookId);
        if (book == null)
        {
            return new ApiResponse<BookDetailDto>
            {
                StatusCode = AppStatusCode.NotFound,
                Message = "Book not found"
            };
        }

        bool updated = false;
        if (!string.IsNullOrWhiteSpace(updateBookDto.Title))
        {
            book.Title = updateBookDto.Title;
            updated = true;
        }
        if (!string.IsNullOrWhiteSpace(updateBookDto.Author))
        {
            book.Author = updateBookDto.Author;
            updated = true;
        }
        if (!string.IsNullOrWhiteSpace(updateBookDto.Publisher))
        {
            book.Publisher = updateBookDto.Publisher;
            updated = true;
        }
        if (!string.IsNullOrWhiteSpace(updateBookDto.ImageUrl))
        {
            book.ImageUrl = updateBookDto.ImageUrl;
            updated = true;
        }
        if (!updated)
        {
            return new ApiResponse<BookDetailDto>
            {
                StatusCode = AppStatusCode.BadRequest,
                Message = "At least one property (Title, Author, Publisher, ImageUrl) must be provided to update."
            };
        }
        book.UpdatedAt = DateTime.UtcNow;
        _context.Books.Update(book);
        await _context.SaveChangesAsync();

        BookDetailDto bookResponse = _mapper.Map<BookDetailDto>(book);
        return new ApiResponse<BookDetailDto>
        {
            StatusCode = AppStatusCode.Success,
            Data = bookResponse,
            Message = "Book updated successfully"
        };
    }
}