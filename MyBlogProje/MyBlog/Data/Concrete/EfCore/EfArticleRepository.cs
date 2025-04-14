using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlog.Data.Abstract;
using MyBlog.Entity;

namespace MyBlog.Data.Concrete.EfCore
{
    public class EfArticleRepository : IArticleRepository
    {
        private BlogContext _context;
        public EfArticleRepository(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<Article> Articles => _context.Articles;

        public void CreateArticle(Article article)
        {
            _context.Articles.Add(article);
            _context.SaveChanges();
        }

        public void DeleteArticle(int artId)
        {
            var art = _context.Articles.FirstOrDefault(i=>i.ArticleId == artId);
            if(art !=null)
            {
                _context.Articles.Remove(art);
                _context.SaveChanges();
            }
        }

        public void EditArticle(Article article)
        {
            var entity = _context.Articles.FirstOrDefault(i=>i.ArticleId==article.ArticleId);

            if(entity!=null)
            {
                entity.Title = article.Title;
                entity.Description = article.Description;
                entity.Content = article.Content;
                entity.Url = article.Url;
                entity.Image = article.Image;

                _context.SaveChanges();
            }

        }
    }
}