using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;

namespace LocalDropshipping.Web.Services
{
    public class ImageService : IImageService
    {
        private readonly LocalDropshippingContext context;

        public ImageService(LocalDropshippingContext context)
        {
            this.context = context;
        }

        public Image Add(IFormFile file)
        {
            var base64Content = ConvertIFormFileToBase64(file);
            var image = new Image();
            image.Content = base64Content;
            context.Images.Add(image);
            context.SaveChanges();
            return image;
        }

        public Image? Delete(int imageId)
        {
            var foundImage = context.Images.FirstOrDefault(x => x.Id == imageId);
            if (foundImage != null)
            {
                context.Images.Remove(foundImage);
                context.SaveChanges();
            }
            return foundImage;
        }

        public string ConvertIFormFileToBase64(IFormFile file)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();
                string base64String = Convert.ToBase64String(fileBytes);
                return base64String;
            }
        }
    }
}
