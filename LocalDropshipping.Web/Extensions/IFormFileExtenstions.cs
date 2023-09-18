using Humanizer;

namespace LocalDropshipping.Web.Extensions
{
    public static class IFormFileExtenstions
    {
        public static string SaveTo(this IFormFile file, string path, string filename)
        {
            filename = Guid.NewGuid().ToString() + "_" + filename.Camelize();
            string uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path);
            string extension = Path.GetExtension(file.FileName);
            using FileStream fs = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create);

            file.CopyTo(fs);
            return @$"\{path}\" + filename + extension;
        }
        public static List<string> SaveTo(this IFormFile[] files, string path, string filename)
        {
            List<string> links = new List<string>();
            foreach (var file in files)
            {
                links.Add(file.SaveTo(path, filename));
            }
            return links;
        }
    }
}
