using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Abstract;
using MyBlog.Entity;

namespace MyBlog.Data.Concrete.EfCore
{
    public class EfCategoryRepository : ICategoryRepository
    {
        private BlogContext _context;
        public EfCategoryRepository(BlogContext context)
        {
            _context = context;
        }
        public IQueryable<Category> Categories => _context.Categories;

        public void CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void EditCategory(Category category)
        {
            var entity = _context.Categories.FirstOrDefault(i => i.CategoryId == category.CategoryId);

            if (entity != null)
            {
                entity.CategoryName = category.CategoryName;
                entity.Url = category.Url;

                _context.SaveChanges();
            }
        }

        
    }
}