using AutoMapper;
using ECommerceApp.ApiResponse;
using lmsApi.Constants.AppStatusCode;
using lmsApi.Data;
using lmsApi.Models.Entities;
using lmsApi.Models.Dtos.BookCategory;
using Microsoft.EntityFrameworkCore;

namespace lmsApi.Services;

public interface IBookCategoryService
{
    Task<ApiResponse<BookCategoryDetail>> CreateBookCategory(CreateBookCategory createCategory);
    Task<ApiResponse<List<BookCategoryDetail>>> GetAllCategories();
    Task<ApiResponse<BookCategoryResponse>> GetCategoryById(int id);
    Task<ApiResponse<BookCategoryDetail>> UpdateCategory(int id, UpdateBookCategoryDto updateCategory);
    Task<ApiResponse<string>> DeleteCategory(int id);
    Task<ApiResponse<List<BookCategoryDetail>>> GetActiveCategories();
}

public class BookCategoryService : IBookCategoryService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public BookCategoryService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<BookCategoryDetail>> CreateBookCategory(CreateBookCategory createCategory)
    {

        var existingCategory = await _context.Categories
            .FirstOrDefaultAsync(c => c.Name.ToLower() == createCategory.Name.ToLower());

        if (existingCategory != null)
        {
            return new ApiResponse<BookCategoryDetail>
            {
                StatusCode = AppStatusCode.BadRequest,
                Message = "Category with this name already exists",
                Errors = new List<string> { "Duplicate category name" }
            };
        }


        BookCategory category = _mapper.Map<BookCategory>(createCategory);

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        BookCategoryDetail resultToSend = _mapper.Map<BookCategoryDetail>(category);

        return new ApiResponse<BookCategoryDetail>
        {
            StatusCode = AppStatusCode.Created,
            Data = resultToSend,
            Message = "Category created successfully"
        };

    }

    public async Task<ApiResponse<List<BookCategoryDetail>>> GetAllCategories()
    {

       List<BookCategory> categories = await _context.Categories
            .OrderBy(c => c.Name)
            .ToListAsync();

       List<BookCategoryDetail> resultToSend = _mapper.Map<List<BookCategoryDetail>>(categories);

        return new ApiResponse<List<BookCategoryDetail>>
        {
            StatusCode = AppStatusCode.Success,
            Data = resultToSend,
            Message = "Categories retrieved successfully"
        };

    }

    public async Task<ApiResponse<BookCategoryResponse>> GetCategoryById(int id)
    {

        var category = await _context.Categories
            .Include(c => c.Books).ThenInclude(b=>b.BookCopies)
            .FirstOrDefaultAsync(c => c.CategoryId == id);

        if (category == null)
        {
            return new ApiResponse<BookCategoryResponse>
            {
                StatusCode = AppStatusCode.NotFound,
                Message = "Category not found"
            };
        }
        
        BookCategoryResponse responseToSend=new BookCategoryResponse()
        {
            CategoryId = category.CategoryId,   
            Name = category.Name,
            Description = category.Description ?? "",
            ImageUrl = category.ImageUrl,
            Books = _mapper.Map<List<BookDetailForCategory>>(category.Books)
        };

        return new ApiResponse<BookCategoryResponse>
        {
            StatusCode = AppStatusCode.Success,
            Data = responseToSend,
            Message = "Category retrieved successfully"
        };

    }

    public async Task<ApiResponse<List<BookCategoryDetail>>> GetActiveCategories()
    {

        var categories = await _context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync();

        List<BookCategoryDetail> resultToSend = _mapper.Map<List<BookCategoryDetail>>(categories);

        return new ApiResponse<List<BookCategoryDetail>>
        {
            StatusCode = AppStatusCode.Success,
            Data = resultToSend,
            Message = "Active categories retrieved successfully"
        };

    }

    public async Task<ApiResponse<BookCategoryDetail>> UpdateCategory(int id, UpdateBookCategoryDto updateCategory)
    {

        var category = await _context.Categories.FindAsync(id);

        if (category == null)
        {
            return new ApiResponse<BookCategoryDetail>
            {
                StatusCode = AppStatusCode.NotFound,
                Message = "Category not found"
            };
        }


        if (!string.IsNullOrWhiteSpace(updateCategory.Name))
        {
            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == updateCategory.Name.ToLower() && c.CategoryId != id);

            if (existingCategory != null)
            {
                return new ApiResponse<BookCategoryDetail>
                {
                    StatusCode = AppStatusCode.BadRequest,
                    Message = "Another category with this name already exists",
                    Errors = new List<string> { "Duplicate category name" }
                };
            }
        }


        if (!string.IsNullOrWhiteSpace(updateCategory.Name))
        {
            category.Name = updateCategory.Name.Trim();
        }

        if (updateCategory.Description != null)
        {
            category.Description = string.IsNullOrWhiteSpace(updateCategory.Description)
                ? null
                : updateCategory.Description.Trim();
        }

        if (updateCategory.ImageUrl != null)
        {
            category.ImageUrl = string.IsNullOrWhiteSpace(updateCategory.ImageUrl)
                ? null
                : updateCategory.ImageUrl.Trim();
        }

        if (updateCategory.IsActive.HasValue)
        {
            category.IsActive = updateCategory.IsActive.Value;
        }

        category.UpdatedAt = DateTime.UtcNow;

        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
        BookCategoryDetail categoryDetail = _mapper.Map<BookCategoryDetail>(category);

        return new ApiResponse<BookCategoryDetail>
        {
            StatusCode = AppStatusCode.Success,
            Data = categoryDetail,
            Message = "Category updated successfully"
        };


    }

    public async Task<ApiResponse<string>> DeleteCategory(int id)
    {

        var category = await _context.Categories
            .FindAsync(id);

        if (category == null || !category.IsActive)
        {
            return new ApiResponse<string>
            {
                StatusCode = AppStatusCode.NotFound,
                Message = "Category not found"
            };
        }

        category.IsActive = false;
        _context.Update(category);
        await _context.SaveChangesAsync();

        return new ApiResponse<string>
        {
            StatusCode = AppStatusCode.Success,
            Data = "Category deleted successfully",
            Message = "Category deleted successfully"
        };

    }
}
