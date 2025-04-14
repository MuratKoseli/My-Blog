using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlog.Entity;

namespace MyBlog.Models
{
    public class PostViewModel
    {
        public List<Post>? Posts { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}