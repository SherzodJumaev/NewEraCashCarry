using NewEraCashCarry.DTOs.ProductDtos;
using PDP.University.Examine.Project.Web.API.DTOs.ProductDtos;
using PDP.University.Examine.Project.Web.API.Models;

namespace PDP.University.Examine.Project.Web.API.Mappers.ProductMaps
{
    public static class ProductMappers
    {
        public static ProductDto FromProductToProductDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
            };
        }

        public static ProductDtoForV2 FromProductToProductDtoForV2(this Product product)
        {
            return new ProductDtoForV2
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl
            };
        }

        public static Product CreateFromProductDtoToProduct(this CreateProductRequestDto requestDto)
        {
            return new Product
            {
                Name = requestDto.Name,
                Description = requestDto.Description,
                Price = requestDto.Price,
                Stock = requestDto.Stock,
                CategoryId = requestDto.CategoryId,
            };
        }
    }
}
