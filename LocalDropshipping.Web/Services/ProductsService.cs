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
                                .Include(x => x.Variants)
                                .ToList();
            return data;
        }

        public Product Add(Product product)
        {
            var userEmail = _userService.GetCurrentUserAsync().GetAwaiter().GetResult()!.Email;

            product.CreatedDate = DateTime.Now;
            product.CreatedBy = userEmail;

            foreach (var variant in product.Variants)
            {
                variant.CreatedDate = DateTime.Now;
                variant.CreatedBy = userEmail;

                // TODO: Add Image Upload Functionality (Zubair)
                variant.FeatureImageLink = "https://picsum.photos/400/400";
            }

            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public Product? GetById(int productId)
        {
            return _context.Products.Include(x => x.Category).Include(x => x.Variants).FirstOrDefault(p => p.ProductId == productId && !p.IsDeleted);
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
                var userEmail = _userService.GetCurrentUserAsync().GetAwaiter().GetResult()!.Email;

                exProduct.Name = product.Name;
                exProduct.Description = product.Description;
                exProduct.SKU = product.SKU;
                exProduct.IsDeleted = product.IsDeleted;
                exProduct.CategoryId = product.CategoryId;
                exProduct.IsFeatured = product.IsFeatured;
                exProduct.IsBestSelling = product.IsBestSelling ;
                exProduct.IsNewArravial = product.IsNewArravial;

                foreach (var variant in product.Variants)
                {
                    if (variant.VariantId == 0)
                    {
                        variant.CreatedDate = DateTime.Now;
                        variant.CreatedBy = userEmail;
                        exProduct.Variants.Add(variant);
                    }
                    else
                    {
                        var exVariant = product.Variants.First(x => x.VariantId == variant.VariantId);
                        exVariant.VariantType = variant.VariantType;
                        exVariant.VariantPrice = variant.VariantPrice;
                        exVariant.Quantity = variant.Quantity;
                        exVariant.UpdatedDate = DateTime.Now;
                        exVariant.UpdatedBy = userEmail;
                    }
                }


                exProduct.UpdatedDate = DateTime.Now;
                exProduct.UpdatedBy = userEmail;
                _context.SaveChanges();
            }
            return exProduct;
        }

        public List<Product> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            // TODO: Needs to be updated
            //return _context.Products.Where(x => x.IsDeleted == false && x.Price >= minPrice && x.Price <= maxPrice && !x.IsDeleted).ToList();
            return _context.Products.ToList();
        }
    }

}
