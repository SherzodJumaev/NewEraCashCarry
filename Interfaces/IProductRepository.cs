using PDP.University.Examine.Project.Web.API.DTOs.ProductDtos;
using PDP.University.Examine.Project.Web.API.Helpers;
using PDP.University.Examine.Project.Web.API.Models;

namespace PDP.University.Examine.Project.Web.API.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync(ProductQueryObject query);
        Task<Product> GetByIdAsync(int id);
        Task<Product> CreateAsync(int categoryId, Product product);
        Task<Product> UpdateAsync(int id, UpdateProductRequestDto product);
        Task<Product> DeleteAsync(int id);
        Task<bool> ProductExists(int id);
    }
}
