using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Models
{
    public class CategoryCreateViewModel
    {
        public int CategoryId { get; set; }
        
        [Required]
        [Display(Name = "Kategori İsmi")]
        public string? CategoryName { get; set; }

        [Required]
        [Display(Name = "Url")]
        public string? Url { get; set; }
    }
}