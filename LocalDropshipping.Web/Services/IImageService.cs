using LocalDropshipping.Web.Data.Entities;

namespace LocalDropshipping.Web.Services
{
    public interface IImageService
    {
        Image Add(IFormFile file);
        string ConvertIFormFileToBase64(IFormFile file);
        Image? Delete(int imageId);
    }
}