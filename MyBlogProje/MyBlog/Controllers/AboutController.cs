using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Abstract;

namespace MyBlog.Controllers
{
    public class AboutController :Controller
    {
        public IAboutRepository _aboutRepository;

        public AboutController(IAboutRepository aboutRepository)
        {
            _aboutRepository = aboutRepository;
        }

        public IActionResult Index()
        {
            var about = _aboutRepository.Abouts.FirstOrDefault();
            return View(about);
        }
    }
}