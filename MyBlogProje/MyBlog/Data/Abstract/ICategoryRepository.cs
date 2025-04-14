using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlog.Data.Concrete.EfCore;
using MyBlog.Entity;

namespace MyBlog.Data.Abstract
{
    public interface ICategoryRepository
    {
        IQueryable<Category> Categories { get; }
        void CreateCategory(Category category);
        void EditCategory(Category category);
    }
}