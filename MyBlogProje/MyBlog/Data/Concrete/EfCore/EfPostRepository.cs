using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Abstract;
using MyBlog.Entity;

namespace MyBlog.Data.Concrete.EfCore
{
    public class EfPostRepository : IPostRepository
    {

        private BlogContext _context;

        public EfPostRepository(BlogContext context)
        {
            _context = context;
        }
        public IQueryable<Post> Posts => _context.Posts;

        public void CreatePost(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        public void DeletePost(int postId)
        {
            var post = _context.Posts.FirstOrDefault(i => i.PostId == postId);
            if (post != null)
            {
                _context.Posts.Remove(post);
                _context.SaveChanges();
            }
        }

        public void EditPost(Post post)
        {
            var entity = _context.Posts.FirstOrDefault(i => i.PostId == post.PostId);

            if (entity != null)
            {
                entity.Title = post.Title;
                entity.Description = post.Description;
                entity.Content = post.Content;
                entity.Url = post.Url;
                entity.Image = post.Image;

                _context.SaveChanges();
            }
        }

        public List<Post> GetAllPosts()
        {
            return _context.Posts.OrderByDescending(p => p.CreatedAt).AsNoTracking().ToList();
        }

        public async Task<List<Post>> GetPostsByCategoryAsync(int categoryId)
        {
            var posts = await _context.Posts
                                   .Where(p => p.CategoryId == categoryId)
                                   .OrderByDescending(p => p.CreatedAt)
                                   .ToListAsync();

            return posts;
        }
    }
}