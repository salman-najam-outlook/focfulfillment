using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
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
            var data = context.Products.Include(x => x.Category).Where(x => x.IsDeleted == false).Take(10).ToList();
            if (data.Count > 0)
            {
                return data;
            }
            return new List<Product>
            { new Product() };  
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

        public Product? Delete(int productId)
        {
            var product = context.Products.FirstOrDefault(x => x.ProductId == productId);
            if (product != null)
            {
                context.Products.Remove(product);
               // product.IsDeleted = true;
                context.SaveChanges();
            }
            return product;
        }

        public Product? Update(int productId, ProductDto productDto)
        {
            var exProduct = context.Products.FirstOrDefault(x => x.ProductId == productId);
            if (exProduct != null)
            {
                exProduct.Price = productDto.Price;
                exProduct.Name = productDto.Name;
                exProduct.Description = productDto.Description;
                exProduct.ImageContent = productDto.ImageLink;
                exProduct.Quantity = productDto.Stock;
                exProduct.UpdatedDate = productDto.UpdatedDate;
                exProduct.CreatedDate = productDto.CreatedDate;
                exProduct.SKU = productDto.SKU;
                context.SaveChanges();
            }
            return exProduct;
        }

        public List<Product> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return context.Products.Where(x => x.IsDeleted == false && x.Price >= minPrice && x.Price <= maxPrice).ToList();
        }
    }

}
