using Microsoft.AspNetCore.Mvc;
using PDP.University.Examine.Project.Web.API.Models;

namespace NewEraCashCarry.Interfaces
{
    public interface IImageRepository
    {
        Task<Product> UploadImage(int id, IFormFile file);
    }
}
