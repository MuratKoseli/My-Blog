using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlog.Entity;

namespace MyBlog.Models
{
    public class PostCategoryViewModel
    {
        public Category? Category { get; set; } 
    public List<Post>? Posts { get; set; }
    }
}