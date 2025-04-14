using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyBlog.Entity;

namespace MyBlog.Data.Concrete.EfCore
{
    public class BlogContext : IdentityDbContext<IdentityUser>
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {

        }
        // Bu constructor, Bağımlılık Enjeksiyonu (Dependency Injection, DI) ile veritabanı bağlantı ayarlarını alır ve DbContext'e aktarır. Startup.cs veya Program.cs dosyasında, uygulamaya hangi veritabanının kullanılacağını belirtirken bu seçenekler belirlenir.


        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<About> Abouts => Set<About>();
        public DbSet<Article> Articles => Set<Article>();
    }
}

// Bu kod, Entity Framework Core kullanarak bir veritabanı bağlamı (DbContext) tanımlar. Yani, veritabanı ile ASP.NET Core uygulaman arasındaki bağlantıyı ve işlemleri yönetmek için kullanılır.
// Bu BlogContext sınıfı sayesinde:
// -Entity Framework Core kullanarak veritabanını C# nesneleri ile yönetebilirsin.
// -DbSet<T> ile veritabanındaki tabloların C# sınıfları ile eşlenmesini sağlarsın.
// -DbContext aracılığıyla veritabanına bağlanabilir, veri ekleyebilir, güncelleyebilir veya sorgular çalıştırabilirsin.