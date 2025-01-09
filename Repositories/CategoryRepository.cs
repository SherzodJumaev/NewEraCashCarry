using Microsoft.EntityFrameworkCore;
using PDP.University.Examine.Project.Web.API.Data;
using PDP.University.Examine.Project.Web.API.DTOs.CategoryDTOs;
using PDP.University.Examine.Project.Web.API.Helpers;
using PDP.University.Examine.Project.Web.API.Interfaces;
using PDP.University.Examine.Project.Web.API.Mappers.CategoryMaps;
using PDP.University.Examine.Project.Web.API.Models;

namespace PDP.University.Examine.Project.Web.API.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDBContext _context;
        public CategoryRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<bool> CategoryExists(int id)
        {
            if (!await _context.Categories.AnyAsync(c => c.Id == id))
            {
                return false;
            }

            return true;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<Category> DeleteAsync(int id)
        {
            var foundCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (foundCategory == null)
            {
                return null;
            }

            _context.Categories.Remove(foundCategory);
            await _context.SaveChangesAsync();

            return foundCategory;
        }

        public async Task<IEnumerable<Category>> GetAllAsync(CategoryQueryObject query)
        {
            var categories = _context.Categories.Include(c => c.Products).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                categories = categories.Where(c => c.Name.Contains(query.Name));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    categories = query.IsDescending ? categories.OrderByDescending(c => c.Name) : categories.OrderBy(c => c.Name);
                }
            }

            return await categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            var foundCategory = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (foundCategory == null)
            {
                return null;
            }

            return foundCategory;
        }

        public async Task<CategoryUpdateDto> UpdateAsync(int id, CategoryUpdateDto categoryDto)
        {
            var foundCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (foundCategory == null)
            {
                return null!;
            }

            foundCategory.Id = id;
            foundCategory.Name = categoryDto.Name;
            foundCategory.Description = categoryDto.Description;

            await _context.SaveChangesAsync();

            return foundCategory.UpdateFromCategoryDtoToCategoryDto();
        }
    }
}
