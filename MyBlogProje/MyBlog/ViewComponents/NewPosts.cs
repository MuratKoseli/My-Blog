using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyBlog.Data.Abstract;

namespace MyBlog.ViewComponents
{
    public class NewPosts : ViewComponent
    {
          private IPostRepository _repository;

        public NewPosts(IPostRepository repository)
        {
            _repository = repository;
        }

        public async Task<IViewComponentResult>  InvokeAsync()
        {
            return View(await  _repository
                                .Posts
                                .OrderByDescending(p=>p.CreatedAt)
                                .Take(5)
                                .ToListAsync());
        }

    }
}