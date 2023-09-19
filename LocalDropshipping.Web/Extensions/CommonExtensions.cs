using LocalDropshipping.Web.Data.Entities;
using System.IO;

namespace LocalDropshipping.Web.Extensions
{
    public static class CommonExtensions
    {
        public static void DeleteFromServer(this ProductVariantImage image, string root)
        {
            try
            {
                string filePath = root + "\\wwwroot" + image.Link;
                File.Delete(filePath);
            }
            catch (Exception ex) 
            {
            }
            return;
        }

        public static void DeleteAllFromServer(this List<ProductVariantImage> images, string root)
        {
            images.ForEach(x => x.DeleteFromServer(root));
        }

        public static void DeleteFromServer(this ProductVariantVideo video, string root)
        {
            try
            {
                string filePath = root + "\\wwwroot" + video.Link;
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
            }
            return;
        }

        public static void DeleteAllFromServer(this List<ProductVariantVideo> videos, string root)
        {
            videos.ForEach(x => x.DeleteFromServer(root));
        }
    }
}
