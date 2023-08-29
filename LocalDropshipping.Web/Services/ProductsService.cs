using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
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
            return context.Products.Include(x => x.Category).Where(x => x.IsDeleted == false).ToList();
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
            try
            {
                var products = context.Products.Find(productId);
                if (products != null)
                {
                    products.IsDeleted = true;
                    context.SaveChanges();
                    return products;
                }
            }
            catch (Exception ex)
            {
            }
            return null;

        }

        public Product? Update(int productId, ProductDto productDto)
        {
            var exProduct = context.Products.FirstOrDefault(x => x.ProductId == productId);
            if (exProduct != null)
            {
                exProduct.Price = productDto.Price;
                exProduct.Name = productDto.Name;
                exProduct.Description = productDto.Description;
                exProduct.ImageLink = productDto.ImageLink;
                exProduct.Stock = productDto.Stock;
                context.SaveChanges();
            }
            return exProduct;
        }
    }

}
