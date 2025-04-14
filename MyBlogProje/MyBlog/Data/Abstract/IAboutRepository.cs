using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlog.Entity;

namespace MyBlog.Data.Abstract
{
    public interface IAboutRepository
    {
        IQueryable<About> Abouts {get;}
        void CreateAbout (About about);
        void EditAbout(About about);
    }
}