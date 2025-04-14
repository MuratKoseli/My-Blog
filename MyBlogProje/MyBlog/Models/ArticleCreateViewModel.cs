using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Models
{
    public class ArticleCreateViewModel
    {
        public int ArticleId { get; set; }

        [Required]
        [Display(Name = "Başlık")]
        public string? Title { get; set; }

        [Required]
        [Display(Name = "İçerik")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Açıklama")]
        public string? Content { get; set; }

        [Required]
        [Display(Name = "Url")]
        public string? Url { get; set; }

         public DateTime CreatedAt { get; set; } = DateTime.Now;

        // [Required]
        [Display(Name = "Resim")]
        public string? Image { get; set; }

    }
}