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
            var userEmail = (await _userService.GetCurrentUserAsync()).Email;
            
            productvariant.CreatedBy = userEmail;
            productvariant.CreatedDate = DateTime.Now ;
            
            _context.Add(productvariant);
            await _context.SaveChangesAsync();
            
            return productvariant;
        }

        public async Task<ProductVariant?> GetByIdAsync(int productVariantId)
        {
            return await _context.ProductVariants.Include(x => x.Images).Include(x => x.Videos).FirstOrDefaultAsync(p => p.ProductVariantId == productVariantId);
        }

        public async Task<ProductVariant?> Delete(int productVariantId)
        {
            var productVariant = await _context.ProductVariants.FirstOrDefaultAsync(x => x.ProductVariantId == productVariantId);
            if (productVariant != null)
            {
                productVariant = await Update(productVariantId, productVariant);
            }
            return productVariant;
        }

        public async Task<ProductVariant?> Update(int productId, ProductVariant product)
        {
            var 
                exProductVariant = await _context.ProductVariants.FirstOrDefaultAsync(x => x.ProductVariantId == productId);
            if (exProductVariant != null)
            {
                var userEmail = (await _userService.GetCurrentUserAsync()).Email;

                exProductVariant.VariantPrice = product.VariantPrice;
                exProductVariant.DiscountedPrice = product.DiscountedPrice;
                exProductVariant.Quantity = product.Quantity;
                exProductVariant.VariantType = product.VariantType;
                exProductVariant.CreatedBy = userEmail;
                exProductVariant.CreatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            return exProductVariant;
        }
    }
}