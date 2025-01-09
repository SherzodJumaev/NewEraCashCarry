using PDP.University.Examine.Project.Web.API.DTOs.CategoryDTOs;
using PDP.University.Examine.Project.Web.API.Helpers;
using PDP.University.Examine.Project.Web.API.Models;

namespace PDP.University.Examine.Project.Web.API.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync(CategoryQueryObject query);
        Task<Category> GetByIdAsync(int id);
        Task<Category> CreateAsync(Category category);
        Task<CategoryUpdateDto> UpdateAsync(int id, CategoryUpdateDto categoryDto);
        Task<Category> DeleteAsync(int id);
        Task<bool> CategoryExists(int id);
    }
}
