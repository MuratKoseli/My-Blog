using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Data.Concrete.EfCore
{
    public static class SeedData
    {
        public static async Task TestVerileriniDoldur(IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetService<BlogContext>();

            var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

            if (context != null)
            {
                // Veritabanını güncellemeyi sağla
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                // Kategoriler ekle
                if (!context.Categories.Any())
                {
                    context.Categories.AddRange
                    (
                        new Entity.Category { CategoryName = "Şiir", Url = "siir" }
                    );
                    await context.SaveChangesAsync();
                }

                // Makaleler ekle
                if (!context.Articles.Any())
                {
                    context.Articles.AddRange
                    (
                        new Entity.Article
                        {
                            Title = "Edebiyat Hakkında",
                            Content = "Edebiyat iyidir.",
                            Description = "Edebiyat Nedir?",
                            Url = "edebiyat-hakkında",
                            Image = "hm.jpg",
                            CreateTime = DateTime.UtcNow // Ensure UTC
                        },
                        new Entity.Article
                        {
                            Title = "Türk Edebiyatı",
                            Content = "Türk edebiyatı, çok köklü bir tarihe sahiptir.",
                            Description = "Türk Edebiyatının Tarihi",
                            Url = "türk-edebiyatı-hakkında",
                            Image = "hm2.jpg",
                            CreateTime = DateTime.UtcNow // Ensure UTC
                        }
                    );
                    await context.SaveChangesAsync();

                    if(!context.Abouts.Any())
                    {
                        context.Abouts.AddRange
                        (
                            new Entity.About
                            {
                                Title = "Hakkımda",
                                Content = "Murat Köşeli",
                                Image = "mk.jpg"
                            }
                        );
                        await context.SaveChangesAsync();
                    }
                    

                    // Eğer postlar yoksa, ekle
                    if (!context.Posts.Any())
                    {
                        context.Posts.AddRange
                        (
                            new Entity.Post
                            {
                                Title = "Şiir 1",
                                Description = "birinci yazı",
                                Content = "Ne için var oldum? Bir muammanın peşinde köleyim! Kim seçtirdi bana bu yolu? Anımsadığım her hatıraya lekeyim",
                                Url = "birinci-siir",
                                Image = "WTR.jpg",
                                CreatedAt = DateTime.UtcNow.AddDays(-10),
                                CategoryId = 1
                            },
                            new Entity.Post
                            {
                                Title = "Şiir 2",
                                Description = "ikinci yazı",
                                Content = "Ne için var oldum?",
                                Url = "ikinci-siir",
                                Image = "sea.jpg",
                                CreatedAt = DateTime.UtcNow.AddDays(-15),
                                CategoryId = 1
                            },
                            new Entity.Post
                            {
                                Title = "Şiir 3",
                                Description = "üçüncü yazı",
                                Content = "Bir muammanın peşinde köleyim! Kim seçtirdi bana bu yolu?",
                                Url = "ücüncü-siir",
                                Image = "night.jpg",
                                CreatedAt = DateTime.UtcNow.AddDays(-20),
                                CategoryId = 1
                            },
                            new Entity.Post
                            {
                                Title = "Şiir 4",
                                Description = "dördüncü yazı",
                                Content = "Bir muammanın peşinde köleyim! Kim seçtirdi bana bu yolu?",
                                Url = "dördüncü-siir",
                                Image = "night.jpg",
                                CreatedAt = DateTime.UtcNow.AddDays(-25),
                                CategoryId = 1
                            }
                        );
                        await context.SaveChangesAsync();
                    }
                }

                // Admin rolünü kontrol et ve oluştur
                var roleExist = await roleManager!.RoleExistsAsync("Admin");
                if (!roleExist)
                {
                    var role = new IdentityRole("Admin");
                    await roleManager.CreateAsync(role);
                }

                // Admin kullanıcısını oluştur
                var user = await userManager!.FindByEmailAsync("admin@admin.com");
                if (user == null)
                {
                    user = new IdentityUser
                    {
                        UserName = "admin@admin.com",
                        Email = "admin@admin.com"
                    };
                    await userManager.CreateAsync(user, "Admin123!");
                }

                // Admin rolünü kullanıcıya ata
                if (user != null && !await userManager.IsInRoleAsync(user, "Admin"))
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
