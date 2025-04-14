using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Abstract;

namespace MyBlog.Controllers
{
    public class HomeController : Controller
    {
         private IArticleRepository _articleRepository;
        public HomeController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }


        public async Task<IActionResult>  Index()
        {
            return View(await _articleRepository.Articles.ToListAsync());
        }

        public async Task<IActionResult> Details(string url)
        {
            var art = await _articleRepository.Articles.FirstOrDefaultAsync(p=>p.Url==url);

            if(art == null)
            {
                return NotFound();
            }
            return View(art);
           
        }
    }
}