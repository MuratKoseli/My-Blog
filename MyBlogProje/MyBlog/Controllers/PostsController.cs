using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Abstract;
using MyBlog.Data.Concrete.EfCore;
using MyBlog.Entity;
using MyBlog.Models;
using X.PagedList;

namespace MyBlog.Controllers
{

    public class PostsController : Controller
    {

        private IPostRepository _postRepository;


        public PostsController(IPostRepository postRepository)
        {
            _postRepository = postRepository;

        }
        

        public async Task<IActionResult> Index(string category, int page = 1, int pageSize = 5)
        {
            var postsQuery = _postRepository.Posts.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                postsQuery = postsQuery.Where(x => x.Category!.CategoryName!.ToLower() == category.ToLower());
            }

            var totalPosts = await postsQuery.CountAsync(); 

            var posts = await postsQuery
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize) 
                .Take(pageSize) 
                .ToListAsync();

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalPosts / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.Category = category;

            return View(posts);
        }




        [HttpGet]


        public async Task<IActionResult> Details(string url)
        {
            var post = await _postRepository.Posts.FirstOrDefaultAsync(p => p.Url == url);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }
    }
}

