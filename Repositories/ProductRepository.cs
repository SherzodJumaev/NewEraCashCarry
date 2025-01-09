using Microsoft.EntityFrameworkCore;
using PDP.University.Examine.Project.Web.API.Data;
using PDP.University.Examine.Project.Web.API.DTOs.ProductDtos;
using PDP.University.Examine.Project.Web.API.Enums;
using PDP.University.Examine.Project.Web.API.Helpers;
using PDP.University.Examine.Project.Web.API.Interfaces;
using PDP.University.Examine.Project.Web.API.Models;

namespace PDP.University.Examine.Project.Web.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ICategoryRepository _categoryRepo;
        public ProductRepository(ApplicationDBContext context, ICategoryRepository categoryRepo)
        {
            _context = context;
            _categoryRepo = categoryRepo;
        }

        public async Task<List<Product>> GetAllAsync(ProductQueryObject query)
        {
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                products = products.Where(p => p.Name.Contains(query.Name));
            }

            if (!query.SortByName.Equals(SortByNameEnum.Name))
            {
                products = query.IsDescending ? products.OrderByDescending(p => p.Name) : products.OrderBy(p => p.Name);
            }

            if (!query.SortByPrice.Equals(ProductEnum.Default))
            {
                products = query.SortByPrice == ProductEnum.Largest ? products.OrderByDescending(p => p.Price) : products.OrderBy(p => p.Price);
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await products.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var foundProduct = await _context.Products.FindAsync(id);

            if (foundProduct == null)
            {
                return null!;
            }

            return foundProduct;
        }

        public async Task<Product> CreateAsync(int categoryId, Product product)
        {
            if (!await _categoryRepo.CategoryExists(categoryId))
            {
                return null!;
            }

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Product> UpdateAsync(int id, UpdateProductRequestDto product)
        {
            var foundProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (foundProduct == null)
            {
                return null!;
            }

            foundProduct.Id = id;
            foundProduct.Name = product.Name!;
            foundProduct.Description = product.Description!;
            foundProduct.Price = product.Price;
            foundProduct.Stock = product.Stock;

            await _context.SaveChangesAsync();

            return foundProduct;
        }

        public async Task<Product> DeleteAsync(int id)
        {
            var productModel = await _context.Products.FindAsync(id);

            if (productModel == null)
            {
                return null!;
            }

            _context.Products.Remove(productModel);
            await _context.SaveChangesAsync();

            return productModel;
        }

        public async Task<bool> ProductExists(int id)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (existingProduct == null)
            {
                return false;
            }

            return true;
        }
    }
}
