using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using Microsoft.EntityFrameworkCore;

namespace LocalDropshipping.Web.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly LocalDropshippingContext context;

        public CategoryService(LocalDropshippingContext context)
        {
            this.context = context;
        }
        public List<Category> GetAll()
        {
            return context.Categories.ToList();
           // return context.Categories.Where(x => x.IsActive == true).ToList();
        }

        public Category Add(Category category)
        {

            //category.IsActive = true;
            context.Categories.Add(category);

            context.SaveChanges();
            return category;
        }

        public Category? GetById(int CategoryId)
        {
            return context.Categories.FirstOrDefault(c => c.CategoryId == CategoryId);
        }

        public Category? Delete(int CategoryId)
        {
            try
            {
                var Category = context.Categories.Find(CategoryId);
                if (Category != null)
                {
                   // Category.IsActive = false;
                    context.SaveChanges();
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
            var category = context.Categories.FirstOrDefault(x => x.CategoryId == CategoryId);
            if (category != null)
            {
                category.Name = categoryDto.Name;
                context.SaveChanges();
            }
            return category;
        }
    }

}



