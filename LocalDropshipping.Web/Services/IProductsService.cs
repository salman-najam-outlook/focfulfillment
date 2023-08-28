using LocalDropshipping.Web.Data.Entities;

namespace LocalDropshipping.Web.Services
{
    public interface IProductsService
    {
        Product Add(Product product);
        List<Product> GetAll();
        Product? GetById(int productId);
    }
}