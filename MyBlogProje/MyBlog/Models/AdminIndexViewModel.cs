using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlog.Entity;

namespace MyBlog.Models
{
    public class AdminIndexViewModel
    {
        public List<Post>? Posts { get; set; }
        public List<Article>? Articles  { get; set; }
    }
}