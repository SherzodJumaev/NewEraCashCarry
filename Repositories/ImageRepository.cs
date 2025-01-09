using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NewEraCashCarry.Interfaces;
using PDP.University.Examine.Project.Web.API.Data;
using PDP.University.Examine.Project.Web.API.Models;

namespace NewEraCashCarry.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ImageRepository(ApplicationDBContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<Product> UploadImage(int id, IFormFile file)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return null;

            var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath ?? "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
            { 
                Directory.CreateDirectory(uploadsFolder); 
            }

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var dbPath = $"/uploads/{fileName}";

            product.ImageUrl = dbPath;
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return product;
        }
    }
}
