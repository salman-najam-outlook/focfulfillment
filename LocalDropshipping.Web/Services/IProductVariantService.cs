
using LocalDropshipping.Web.Data.Entities;

namespace LocalDropshipping.Web.Services
{
    public interface IProductVariantService
    {
        Task<ProductVariant> Add(ProductVariant productVariant);
        List<ProductVariant> GetAll();
        Task<ProductVariant?> GetByIdAsync(int productVariantId);
        Task<ProductVariant> Delete(int productVariantId);
        Task<ProductVariant?> Update(int productVariantId, ProductVariant productVariant);
    }
}
