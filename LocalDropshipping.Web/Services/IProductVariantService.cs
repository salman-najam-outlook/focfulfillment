
using LocalDropshipping.Web.Data.Entities;

namespace LocalDropshipping.Web.Services
{
    public interface IProductVariantService
    {
        Task<ProductVariant> Add(ProductVariant productvariant);
        List<ProductVariant> GetAll();
        Task<ProductVariant?> GetById(int productvariantId);
        Task<ProductVariant> Delete(int productvariantId);
        Task<ProductVariant?> Update(int productvariantId, ProductVariant productvariant);
    }
}
