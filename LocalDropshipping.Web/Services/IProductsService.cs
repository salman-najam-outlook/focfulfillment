using LocalDropshipping.Web.Data.Entities;

namespace LocalDropshipping.Web.Services
{
    public interface IProductsService
    {
        Product Add(Product product);
        List<Product> GetAll();
        Product? GetById(int productId);
        Product Delete(int productId);
        Product? Update(int productId, Product product, bool isSimpleProduct = true);
        List<Product> GetProductsByPriceRange(decimal minPrice, decimal maxPrice);
        List<Product> GetProductsBySearch(string searchString);
    }
}