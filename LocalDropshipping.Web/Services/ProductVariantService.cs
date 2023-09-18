using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis;

namespace LocalDropshipping.Web.Services
{
    public class ProductVariantService : IProductVariantService
    {
        private readonly LocalDropshippingContext _context;
        private readonly IUserService _userService;

        public ProductVariantService(LocalDropshippingContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }


        public List<ProductVariant> GetAll()
        {
            return _context.ProductVariants.ToList();
        }
   

        public async Task<ProductVariant> Add(ProductVariant productvariant)
        {
            var userEmail = await _userService.GetCurrentUserAsync();
            _context.Add(productvariant);
            await _context.SaveChangesAsync();
            return productvariant;
        }

        public async Task<ProductVariant?> GetById(int productId)
        {
            return await _context.ProductVariants.FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task<ProductVariant?> Delete(int productId)
        {
            var product = await _context.ProductVariants.FirstOrDefaultAsync(x => x.ProductId == productId);
            if (product != null)
            {
                product = await Update(productId, product);
            }
            return product;
        }

        public async Task<ProductVariant?> Update(int productId, ProductVariant product)
        {
            var exProduct = await _context.ProductVariants.FirstOrDefaultAsync(x => x.VariantId == productId);
            if (exProduct != null)
            {
                var userEmail = await _userService.GetCurrentUserAsync();

                exProduct.VariantPrice = product.VariantPrice;
                exProduct.Quantity = product.Quantity;
                exProduct.VariantType = product.VariantType;

                await _context.SaveChangesAsync();
            }
            return exProduct;
        }
    }
}