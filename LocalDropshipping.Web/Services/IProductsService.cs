using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Models;

namespace LocalDropshipping.Web.Services
{
    public interface IProductsService
    {
        Product Add(Product product);
        List<Product> GetAll();
        Product? GetById(int productId);
        Product Delete(int productId);
        Product? Update(int productId, ProductDto productDto);
        List<Product> GetProductsByPriceRange(decimal minPrice, decimal maxPrice);
    }
}