using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlog.Data.Abstract;
using MyBlog.Entity;

namespace MyBlog.Data.Concrete.EfCore
{
    public class EfAboutRepository : IAboutRepository
    {
        private BlogContext _context;
        public EfAboutRepository(BlogContext context)
        {
            _context = context;
        }
        public IQueryable<About> Abouts => _context.Abouts;

        public void CreateAbout(About about)
        {
            _context.Abouts.Add(about);
            _context.SaveChanges();
        }

        public void EditAbout(About about)
        {
            var entity = _context.Abouts.FirstOrDefault(i => i.AboutId == about.AboutId);

            if (entity != null)
            {
                entity.Title = about.Title;
                entity.Content = about.Content;
                entity.Image = about.Image;

                _context.SaveChanges();
            }
        }
    }
}