using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDP.University.Examine.Project.Web.API.DTOs.CategoryDTOs;
using PDP.University.Examine.Project.Web.API.Helpers;
using PDP.University.Examine.Project.Web.API.Interfaces;
using PDP.University.Examine.Project.Web.API.Mappers.CategoryMaps;
using Serilog;

namespace PDP.University.Examine.Project.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Retrieves a list of categories based on the provided query parameters.
        /// </summary>
        /// <param name="query">Filtering and pagination options for retrieving categories.</param>
        /// <returns>A list of categories that match the query.</returns>
        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] CategoryQueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var categories = await _categoryRepository.GetAllAsync(query);

                Log.Information($"Categories fetched successfully.");

                return Ok(categories);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to fetch categories.");
                throw; // Let the error middleware handle the exception
            }
        }

        /// <summary>
        /// Retrieves a category by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the category to retrieve.</param>
        /// <returns>
        /// Returns 200 (OK) with the category if found,  
        /// 400 (Bad Request) if the ID is invalid,  
        /// or 404 (Not Found) if no category exists with the given ID.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);

                if (id <= 0)
                {
                    return BadRequest();
                }
                else if (category == null)
                {
                    return NotFound($"Category with the given id:{id} is not found.");
                }

                Log.Information($"Category with the given ID: {id} fetched successfully.");

                return Ok(category);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to fetch Category");
                throw; // Let the error middleware handle the exception
            }
        }

        /// <summary>
        /// Creates a new category based on the provided data.
        /// </summary>
        /// <param name="categoryDto">The category data transfer object containing the information for the new category.</param>
        /// <returns>
        /// Returns 201 (Created) with the created category and its location,  
        /// or appropriate error responses if the creation fails.
        /// </returns>
        [HttpPost]
        [Authorize("Admin")]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var madeCategory = categoryDto.CreateFromCategoryDtoToCategory();
                var createdCategory = await _categoryRepository.CreateAsync(madeCategory);

                Log.Information($"Category {createdCategory.Name} created with ID {createdCategory.Id}");

                return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.Id }, createdCategory);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to create Category {categoryDto.Name}.");
                throw; // Let the error middleware handle the exception
            }
        }

        /// <summary>
        /// Updates an existing category with the provided data.
        /// </summary>
        /// <param name="id">The ID of the category to update.</param>
        /// <param name="categoryDto">The data transfer object containing the updated category information.</param>
        /// <returns>
        /// Returns 200 (OK) with the updated category if successful,  
        /// 400 (Bad Request) if the ID is invalid,  
        /// or 404 (Not Found) if no category exists with the given ID.
        /// </returns>
        [HttpPut("{id}")]
        [Authorize("Admin")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var categoryModel = categoryDto.UpdateFromCategoryDtoToCategory(id);

                var updatedCategory = await _categoryRepository.UpdateAsync(id, categoryModel);

                if (id <= 0)
                {
                    return BadRequest();
                }
                else if (updatedCategory == null)
                {
                    return NotFound($"Category with the given id:{id} is not found.");
                }

                Log.Information($"Category {updatedCategory} update successfully with ID {updatedCategory.Id}");

                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to update category");
                throw; // Let the error middleware handle the exception
            }
        }

        /// <summary>
        /// Deletes a category by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>
        /// Returns 204 (No Content) if the category was successfully deleted,  
        /// or 404 (Not Found) if no category exists with the given ID.
        /// </returns>
        [HttpDelete("{id}")]
        [Authorize("Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id <= 0)
            {
                return BadRequest("Invalid category ID.");
            }

            try
            {
                var category = await _categoryRepository.DeleteAsync(id);

                if (category == null)
                {
                    return NotFound($"Category with the given id:{id} is not found.");
                }

                Log.Information($"Category with ID {id} deleted successfully.");

                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to delete category.");
                throw; // Let the error middleware handle the exception
            }
        }
    }
}
