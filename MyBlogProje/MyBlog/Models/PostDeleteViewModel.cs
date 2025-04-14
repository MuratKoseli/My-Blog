using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Models
{
    public class PostDeleteViewModel
    {
        public int PostId { get; set; }
        
        [Display(Name = "Başlık")]
        public string? Title { get; set; }
    }
}