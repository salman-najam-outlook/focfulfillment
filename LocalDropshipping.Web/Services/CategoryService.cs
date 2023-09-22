using Humanizer;
using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using Microsoft.EntityFrameworkCore;

namespace LocalDropshipping.Web.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly LocalDropshippingContext _context;

        public CategoryService(LocalDropshippingContext context)
        {
            _context = context;
        }
        public List<Category> GetAll()
        {
            return _context.Categories.Where(x => x.IsDeleted == false).ToList();
            // return context.Categories.Where(x => x.IsActive == true).ToList();
        }

        public Category Add(Category category)
        {

            //category.IsActive = true;
            _context.Categories.Add(category);

            _context.SaveChanges();
            return category;
        }

        public Category? GetById(int CategoryId)
        {
            return _context.Categories.FirstOrDefault(c => c.CategoryId == CategoryId);
        }

        public Category? Delete(int CategoryId)
        {
            try
            {
                var Category = _context.Categories.Find(CategoryId);
                if (Category != null)
                {
                    Category.IsDeleted = true;
                    _context.SaveChanges();
                    return Category;
                }
            }
            catch (Exception ex)
            {
            }
            return null;

        }

        public Category? Update(int CategoryId, CategoryDto categoryDto)
        {
            var category = _context.Categories.FirstOrDefault(x => x.CategoryId == CategoryId);
            if (category != null)
            {
                category.Name = categoryDto.Name;
                _context.SaveChanges();
            }
            return category;
        }

        public List<Category> GetCatagoreyBySearch(string searchString)
        {
            return _context.Categories.Where(x => !x.IsDeleted && x.Name.Contains(searchString)).ToList();
        }

        public Category? GetDeafultCategory()
        {
            return _context.Categories.FirstOrDefault(x => x.Name.ToUpper() == "Un-managed");
        }
    }

}



