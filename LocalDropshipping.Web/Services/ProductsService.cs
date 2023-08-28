using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LocalDropshipping.Web.Services
{
    public class ProductsService : IProductsService
    {
        private readonly LocalDropshippingContext context;

        public ProductsService(LocalDropshippingContext context)
        {
            this.context = context;
        }


        public List<Product> GetAll()
        {
            return context.Products.Include(x => x.Category).ToList();
        }

        public Product Add(Product product)
        {
            context.Products.Add(product);
            context.SaveChanges();
            return product;
        }

        public Product? GetById(int productId)
        {
            return context.Products.FirstOrDefault(p => p.ProductId == productId);
        }
    }
}
