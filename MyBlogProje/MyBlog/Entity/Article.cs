namespace MyBlog.Entity;

public class Article
{
   public int ArticleId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
    public string? Content { get; set; }
    public string? Image { get; set; }
    public DateTime CreateTime { get; set; } = DateTime.Now;  // Varsayılan değeri ekledik

}
