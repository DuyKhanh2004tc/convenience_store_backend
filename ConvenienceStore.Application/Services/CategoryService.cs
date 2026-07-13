using System.Text.Json;
using ConvenienceStore.Application.Contracts.External;
using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Application.DTOs.Category;
using ConvenienceStore.Application.Services.Interfaces;
using ConvenienceStore.Domain.Entities;

namespace ConvenienceStore.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IGenericRepository<Category> _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private const string CacheKey = "all_categories";

    public CategoryService(
        IGenericRepository<Category> categoryRepository,
        IUnitOfWork unitOfWork,
        ICacheService cacheService)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        // Check cache first
        var cachedData = await _cacheService.GetAsync(CacheKey);
        if (!string.IsNullOrEmpty(cachedData))
        {
            return JsonSerializer.Deserialize<IEnumerable<CategoryDto>>(cachedData) ?? Array.Empty<CategoryDto>();
        }

        // Fetch from database
        var categories = await _categoryRepository.GetAllAsync();
        var dtos = categories.Select(c => new CategoryDto
        {
            CategoryId = c.CategoryId,
            CategoryName = c.CategoryName,
            Description = c.Description
        }).ToList();

        // Save to cache
        var serializedData = JsonSerializer.Serialize(dtos);
        await _cacheService.SetAsync(CacheKey, serializedData, TimeSpan.FromMinutes(60));

        return dtos;
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) return null;

        return new CategoryDto
        {
            CategoryId = category.CategoryId,
            CategoryName = category.CategoryName,
            Description = category.Description
        };
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto)
    {
        var category = new Category
        {
            CategoryName = dto.CategoryName,
            Description = dto.Description
        };

        var created = await _categoryRepository.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        // Invalidate cache
        await _cacheService.RemoveAsync(CacheKey);

        return new CategoryDto
        {
            CategoryId = created.CategoryId,
            CategoryName = created.CategoryName,
            Description = created.Description
        };
    }

    public async Task<bool> UpdateCategoryAsync(int id, CreateCategoryDto dto)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) return false;

        category.CategoryName = dto.CategoryName;
        category.Description = dto.Description;

        _categoryRepository.Update(category);
        await _unitOfWork.SaveChangesAsync();

        // Invalidate cache
        await _cacheService.RemoveAsync(CacheKey);

        return true;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) return false;

        _categoryRepository.Delete(category);
        await _unitOfWork.SaveChangesAsync();

        // Invalidate cache
        await _cacheService.RemoveAsync(CacheKey);

        return true;
    }
}
