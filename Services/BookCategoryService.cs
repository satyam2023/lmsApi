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
    Task<ApiResponse<BookCategory>> CreateBookCategory(CreateBookCategory createCategory);
    Task<ApiResponse<List<BookCategory>>> GetAllCategories();
    Task<ApiResponse<BookCategory>> GetCategoryById(int id);
    Task<ApiResponse<BookCategory>> UpdateCategory(int id, UpdateBookCategoryDto updateCategory);
    Task<ApiResponse<bool>> DeleteCategory(int id);
    Task<ApiResponse<List<BookCategory>>> GetActiveCategories();
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

    public async Task<ApiResponse<BookCategory>> CreateBookCategory(CreateBookCategory createCategory)
    {

        var existingCategory = await _context.Categories
            .FirstOrDefaultAsync(c => c.Name.ToLower() == createCategory.Name.ToLower());

        if (existingCategory != null)
        {
            return new ApiResponse<BookCategory>
            {
                StatusCode = AppStatusCode.BadRequest,
                Message = "Category with this name already exists",
                Errors = new List<string> { "Duplicate category name" }
            };
        }


        var category = new BookCategory
        {
            Name = createCategory.Name.Trim(),
            Description = createCategory.Description?.Trim(),
            ImageUrl = createCategory.ImageUrl?.Trim(),
            IsActive = createCategory.IsActive,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        return new ApiResponse<BookCategory>
        {
            StatusCode = AppStatusCode.Created,
            Data = category,
            Message = "Category created successfully"
        };

    }

    public async Task<ApiResponse<List<BookCategory>>> GetAllCategories()
    {

        var categories = await _context.Categories
            .OrderBy(c => c.Name)
            .ToListAsync();

        return new ApiResponse<List<BookCategory>>
        {
            StatusCode = AppStatusCode.Success,
            Data = categories,
            Message = "Categories retrieved successfully"
        };

    }

    public async Task<ApiResponse<BookCategory>> GetCategoryById(int id)
    {

        var category = await _context.Categories
            .FindAsync(id);

        if (category == null)
        {
            return new ApiResponse<BookCategory>
            {
                StatusCode = AppStatusCode.NotFound,
                Message = "Category not found"
            };
        }

        return new ApiResponse<BookCategory>
        {
            StatusCode = AppStatusCode.Success,
            Data = category,
            Message = "Category retrieved successfully"
        };

    }

    public async Task<ApiResponse<List<BookCategory>>> GetActiveCategories()
    {

        var categories = await _context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync();

        return new ApiResponse<List<BookCategory>>
        {
            StatusCode = AppStatusCode.Success,
            Data = categories,
            Message = "Active categories retrieved successfully"
        };

    }

    public async Task<ApiResponse<BookCategory>> UpdateCategory(int id, UpdateBookCategoryDto updateCategory)
    {

        var category = await _context.Categories.FindAsync(id);

        if (category == null)
        {
            return new ApiResponse<BookCategory>
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
                return new ApiResponse<BookCategory>
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

        return new ApiResponse<BookCategory>
        {
            StatusCode = AppStatusCode.Success,
            Data = category,
            Message = "Category updated successfully"
        };


    }

    public async Task<ApiResponse<bool>> DeleteCategory(int id)
    {

        var category = await _context.Categories
            .FindAsync(id);

        if (category == null)
        {
            return new ApiResponse<bool>
            {
                StatusCode = AppStatusCode.NotFound,
                Message = "Category not found"
            };
        }

        category.IsActive = false;
        _context.Update(category);
        await _context.SaveChangesAsync();

        return new ApiResponse<bool>
        {
            StatusCode = AppStatusCode.Success,
            Data = true,
            Message = "Category deleted successfully"
        };

    }
}
