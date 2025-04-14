using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlog.Entity;

namespace MyBlog.Data.Abstract
{
    public interface IArticleRepository
    {
        IQueryable<Article> Articles {get;}
        void CreateArticle (Article article);
        void EditArticle (Article article);
        void DeleteArticle (int artId);
    }
}