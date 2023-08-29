
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;

namespace LocalDropshipping.Web.Services
{
    public interface ICategoryService
    {
        Category Add(Category category);

        List<Category> GetAll();

        Category? GetById(int CategoryId);

        Category Delete(int CategoryId);
        Category Update(int CategoryId, CategoryDto categoryDto);



    }
}
