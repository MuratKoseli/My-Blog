using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Abstract;
using MyBlog.Entity;
using MyBlog.Models;

namespace MyBlog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private IPostRepository _postRepository;
        private IArticleRepository _articleRepository;
        private ICategoryRepository _categoryRepository;
        private IAboutRepository _aboutRepository;

        public AdminController(IPostRepository postRepository, IArticleRepository articleRepository, ICategoryRepository categoryRepository, IAboutRepository aboutRepository)
        {
            _postRepository = postRepository;
            _articleRepository = articleRepository;
            _categoryRepository = categoryRepository;
            _aboutRepository = aboutRepository;
        }


        public async Task<IActionResult> Index()
        {
            var posts = await _postRepository.Posts.ToListAsync();
            var articles = await _articleRepository.Articles.ToListAsync();
            var viewModel = new AdminIndexViewModel
            {
                Posts = posts,
                Articles = articles
            };
            return View(viewModel);
        }

        public IActionResult ManagePosts()
        {
            var posts = _postRepository.Posts.ToList();
            return View(posts);
        }

        public IActionResult ManageArticles()
        {
            var articles = _articleRepository.Articles.ToList();
            return View(articles);
        }

        public IActionResult ManageCategories()
        {
            var categories = _categoryRepository.Categories.ToList();
            return View(categories);
        }

        public IActionResult ManageAbout()
        {
            var about = _aboutRepository.Abouts.ToList();
            return View(about);
        }


        [HttpGet]
        public IActionResult CreateArt()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateArt(ArticleCreateViewModel model, IFormFile imageFile)
        {
            string? randomFileName = null;

            if (imageFile != null)
            {
                var extension = Path.GetExtension(imageFile.FileName);
                randomFileName = string.Format($"{Guid.NewGuid()}{extension}");
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                if (ModelState.IsValid)
                {

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }
                }

                if (ModelState.IsValid)
                {
                    var art = new Article
                    {
                        Title = model.Title,
                        Description = model.Description,
                        Content = model.Content,
                        Url = model.Url,
                        CreateTime = DateTime.UtcNow,
                        Image = randomFileName
                    };

                    _articleRepository.CreateArticle(art);

                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult CreatePost()
        {
            var categories = _categoryRepository.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                }).ToList();

            ViewBag.Categories = categories;


            return View();
        }


        [HttpPost]
        public IActionResult CreatePost(PostCreateViewModel model, IFormFile imageFile)
        {
            string? randomFileName = null;

            if (imageFile != null)
            {
                var extension = Path.GetExtension(imageFile.FileName);
                randomFileName = string.Format($"{Guid.NewGuid()}{extension}");
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                if (ModelState.IsValid)
                {

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }
                }

            }
            if (ModelState.IsValid)
            {
                var post = new Post
                {
                    Title = model.Title,
                    Description = model.Description,
                    Content = model.Content,
                    Url = model.Url,
                    CreatedAt = DateTime.UtcNow,
                    CategoryId = model.CategoryId,
                    Image = randomFileName
                };

                _postRepository.CreatePost(post);

                var selectedCategory = _categoryRepository.Categories.FirstOrDefault(c => c.CategoryId == model.CategoryId)?.CategoryName;

                if (!string.IsNullOrEmpty(selectedCategory))
                {
                    return RedirectToAction("Index", "Posts", new { category = selectedCategory });
                }

                // return RedirectToAction("Index", "Posts");
                // return RedirectToAction("ManagePosts", "Admin");

            }

            var categories = _categoryRepository.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                }).ToList();

            ViewBag.Categories = categories;

            return View(model);
        }




        [HttpGet]
        public IActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var post = _postRepository.Posts.FirstOrDefault(x => x.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            var categories = _categoryRepository.Categories

            .Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName
            }).ToList();

            ViewBag.Categories = categories;

            return View(new PostCreateViewModel
            {
                PostId = post.PostId,
                Title = post.Title,
                Description = post.Description,
                Content = post.Content,
                Url = post.Url,
                Image = post.Image,
                CategoryId = post.CategoryId

            });
        }

        [HttpPost]
        public IActionResult EditPost(PostCreateViewModel model, IFormFile imageFile)
        {
            string? randomFileName = model.Image; // Başlangıçta mevcut fotoğrafı atıyoruz

            // Fotoğraf seçildiyse, yeni fotoğrafı yükle
            if (imageFile != null)
            {
                var extension = Path.GetExtension(imageFile.FileName);
                randomFileName = string.Format($"{Guid.NewGuid()}{extension}");
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                if (ModelState.IsValid)
                {
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                var entityToUpdate = new Post
                {
                    PostId = model.PostId,
                    Title = model.Title,
                    Description = model.Description,
                    Content = model.Content,
                    Url = model.Url,
                    Image = randomFileName
                };

                _postRepository.EditPost(entityToUpdate);
                return RedirectToAction("Index", "Posts");
            }

            var categories = _categoryRepository.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                }).ToList();

            ViewBag.Categories = categories;
            return View(model);
        }


        [HttpGet]
        public IActionResult EditArticle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var article = _articleRepository.Articles.FirstOrDefault(i => i.ArticleId == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(new ArticleCreateViewModel
            {
                ArticleId = article.ArticleId,
                Title = article.Title,
                Description = article.Description,
                Content = article.Content,
                Url = article.Url,
                Image = article.Image
            });
        }

        [HttpPost]

        public IActionResult EditArticle(ArticleCreateViewModel model, IFormFile imageFile)
        {
            string? randomFileName = model.Image; // Başlangıçta mevcut fotoğrafı atıyoruz

            // Fotoğraf seçildiyse, yeni fotoğrafı yükle
            if (imageFile != null)
            {
                var extension = Path.GetExtension(imageFile.FileName);
                randomFileName = string.Format($"{Guid.NewGuid()}{extension}");
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                if (ModelState.IsValid)
                {
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                var entityToUpdate = new Article
                {
                    ArticleId = model.ArticleId,
                    Title = model.Title,
                    Description = model.Description,
                    Content = model.Content,
                    Url = model.Url,
                    Image = randomFileName
                };

                _articleRepository.EditArticle(entityToUpdate);
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult EditCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = _categoryRepository.Categories.FirstOrDefault(i => i.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(new CategoryCreateViewModel
            {
                CategoryName = category.CategoryName,
                Url = category.Url
            });
        }

        [HttpPost]
        public IActionResult EditCategory(CategoryCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entityToUpdate = new Category
                {
                    CategoryId = model.CategoryId,
                    CategoryName = model.CategoryName,
                    Url = model.Url
                };

                _categoryRepository.EditCategory(entityToUpdate);
                return RedirectToAction("ManageCategories", "Admin");
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult DeletePost(int id)
        {
            var post = _postRepository.Posts.FirstOrDefault(i => i.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(new PostDeleteViewModel
            {
                PostId = post.PostId,
                Title = post.Title,
            });
        }



        [HttpPost]
        public IActionResult DeletePost(PostDeleteViewModel model)
        {
            if (ModelState.IsValid)
            {
                _postRepository.DeletePost(model.PostId);
                return RedirectToAction("ManagePosts", "Admin");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult DeleteArticle(int id)
        {
            var art = _articleRepository.Articles.FirstOrDefault(i => i.ArticleId == id);
            if (art == null)
            {
                return NotFound();
            }
            return View(new ArticleDeleteViewModel
            {
                ArticleId = art.ArticleId,
                Title = art.Title
            });
        }

        [HttpPost]

        public IActionResult DeleteArticle(ArticleDeleteViewModel model)
        {
            if (ModelState.IsValid)
            {
                _articleRepository.DeleteArticle(model.ArticleId);
                return RedirectToAction("ManageArticles", "Admin");
            }
            return View(model);
        }



        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();

        }

        [HttpPost]
        public IActionResult CreateCategory(CategoryCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    CategoryName = model.CategoryName,
                    Url = model.Url
                };

                _categoryRepository.CreateCategory(category);

                // Başarılı ise kullanıcıyı yönlendir
                TempData["SuccessMessage"] = "Kategori başarıyla eklendi.";
                return RedirectToAction("Index");
            }


            // Model geçerli değilse, formu tekrar göster
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateAbout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateAbout(AboutCreateViewModel model)
        {


            if (ModelState.IsValid)
            {
                var about = new About
                {
                    Title = model.Title,
                    Content = model.Content,
                    Image = "mk.jpg"
                };

                _aboutRepository.CreateAbout(about);


                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]

        public IActionResult EditAbout(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var about = _aboutRepository.Abouts.FirstOrDefault(i => i.AboutId == id);
            if (about == null)
            {
                return NotFound();
            }

            return View(new AboutCreateViewModel
            {
                Title = about.Title,
                Content = about.Content,
                Image = about.Image
            });
        }

        [HttpPost]
        public IActionResult EditAbout(AboutCreateViewModel model, IFormFile imageFile)
        {
            string? randomFileName = model.Image; // Başlangıçta mevcut fotoğrafı atıyoruz

            // Fotoğraf seçildiyse, yeni fotoğrafı yükle
            if (imageFile != null)
            {
                var extension = Path.GetExtension(imageFile.FileName);
                randomFileName = string.Format($"{Guid.NewGuid()}{extension}");
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);

                if (ModelState.IsValid)
                {
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                var entityToUpdate = new About
                {
                    Title = model.Title,
                    Content = model.Content,
                    Image = randomFileName
                };

                _aboutRepository.EditAbout(entityToUpdate);
                return RedirectToAction("Index", "About");
            }

            return View(model);
        }


    }
}