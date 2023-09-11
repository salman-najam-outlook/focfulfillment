using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LocalDropshipping.Web.Services
{
    public class ProductsService : IProductsService
    {
        private readonly LocalDropshippingContext _context;
        private readonly IUserService _userService;

        public ProductsService(LocalDropshippingContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public List<Product> GetAll()
        {
            var data = _context.Products
                                .Where(x => !x.IsDeleted)
                                .Include(x => x.Category)
                                .ToList();
            return data;
        }

        public Product Add(Product product)
        {
            product.CreatedDate = DateTime.Now;
            product.CreatedBy = _userService.GetCurrentUserAsync().GetAwaiter().GetResult()!.Email;

            // TODO: Add Image Upload Functionality (Zubair)
            product.ImageLink = "https://picsum.photos/400/400";
            product.CategoryId = 3;

            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public Product? GetById(int productId)
        {
            return _context.Products.FirstOrDefault(p => p.ProductId == productId && !p.IsDeleted);
        }

        public Product? Delete(int productId)
        {
            var product = _context.Products.FirstOrDefault(x => x.ProductId == productId);
            if (product != null)
            {
                product.IsDeleted = true;
                product = Update(productId, product);
            }
            return product;
        }

        public Product? Update(int productId, Product product)
        {
            Product? exProduct = _context.Products.FirstOrDefault(x => x.ProductId == productId);
            if (exProduct != null)
            {
                exProduct.Price = product.Price;
                exProduct.Name = product.Name;
                exProduct.Description = product.Description;
                exProduct.Quantity = product.Quantity;
                exProduct.SKU = product.SKU;
                exProduct.IsDeleted = exProduct.IsDeleted;

                exProduct.UpdatedDate = DateTime.Now;
                exProduct.UpdatedBy = _userService.GetCurrentUserAsync().GetAwaiter().GetResult()!.Email;
                _context.SaveChanges();
            }
            return exProduct;
        }

        public List<Product> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return _context.Products.Where(x => x.IsDeleted == false && x.Price >= minPrice && x.Price <= maxPrice && !x.IsDeleted).ToList();
        }
    }

}
