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
        // BlogContext bağımlılığı controller'a enjekte edilir.
        // _context ile controller içinde veritabanına erişim sağlanır.
        // Dependency Injection sayesinde bağımlılık yönetimi kolaylaşır ve bağımsız test edilebilir kod yazılır.
        //Repository'den sonra buna evrildi.

        public async Task<IActionResult> Index(string category, int page = 1, int pageSize = 5)
        {
            var postsQuery = _postRepository.Posts.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                postsQuery = postsQuery.Where(x => x.Category!.CategoryName!.ToLower() == category.ToLower());
            }

            var totalPosts = await postsQuery.CountAsync(); // Toplam post sayısını al

            var posts = await postsQuery
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize) // Sayfa atlaması yap
                .Take(pageSize) // Sayfadaki öğe sayısını belirle
                .ToListAsync();

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalPosts / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.Category = category;

            return View(posts);
        }



        // public async Task<IActionResult> Index(string category)
        // {

        //     var posts = await _postRepository.Posts.Where(x=> x.Category!.CategoryName!.ToLower() == category.ToLower()).OrderByDescending(p => p.CreatedAt).ToListAsync();
        //     return View(posts);
        //     // return View(await _postRepository.Posts.ToListAsync());


        // }

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



// var allowedExtension = new[] {".jpg", ".jpeg", ".png", ".webp"};
// var extension = Path.GetExtension(imageFile.FileName);
// var randomFileName= string.Format($"{Guid.NewGuid().ToString()}{extension}");
// var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

// if(imageFile!= null){
// if(!allowedExtension.Contains(extension))
// {
//     ModelState.AddModelError("", "Geçerli bi resim seçiniz");
// }