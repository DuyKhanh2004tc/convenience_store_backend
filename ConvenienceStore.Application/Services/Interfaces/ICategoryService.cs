using ConvenienceStore.Application.DTOs.Category;

namespace ConvenienceStore.Application.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
    Task<CategoryDto?> GetCategoryByIdAsync(int id);
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto);
    Task<bool> UpdateCategoryAsync(int id, CreateCategoryDto dto);
    Task<bool> DeleteCategoryAsync(int id);
}
