using System;

namespace MyBlog.Entity;

public class Post
{
   public int PostId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Content { get; set; }
    public string? Url { get; set; }
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;


    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    //Bu ilişki, her yazının bir kategoriye ait olmasını sağlar ve kategori başına birden fazla yazı olabilir.
}
