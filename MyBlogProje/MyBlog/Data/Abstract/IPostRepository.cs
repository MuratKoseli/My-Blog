using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlog.Entity;

namespace MyBlog.Data.Abstract
{
    public interface IPostRepository
    {
        IQueryable<Post> Posts { get; }
        void CreatePost(Post post);
        void EditPost(Post post);
        void DeletePost(int postId);
        List<Post> GetAllPosts();
        Task<List<Post>> GetPostsByCategoryAsync(int categoryId);


    }
}