using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Models
{
    public class AboutCreateViewModel
    {
        public int AboutId { get; set; }
        public string? Title { get; set; }

        public string? Content { get; set; }

        public string? Image { get; set; }

    }
}