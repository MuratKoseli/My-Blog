using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyBlog.Data;
using MyBlog.Data.Abstract;
using MyBlog.Data.Concrete.EfCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// builder.Services.AddDbContext<BlogContext>(options =>
// {
//     var config = builder.Configuration;
//     var connectionString = config.GetConnectionString("pssql_connection");
//     options.UseNpgsql(connectionString);
// });

builder.Services.AddDbContext<BlogContext>(options =>
{
    var config = builder.Configuration;
    var connectionString = config.GetConnectionString("mssql_connection"); // MSSQL için yeni connection string
    options.UseSqlServer(connectionString);
});


// builder.Services.AddDbContext<BlogContext>(options =>{
// options.UseSqlite(builder.Configuration["ConnectionStrings:sqlite_connection"]);
// });

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<BlogContext>()
    .AddDefaultTokenProviders();


builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
});

builder.Services.AddAuthentication()
    .AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
});

builder.Services.AddAuthorization();


builder.Services.AddScoped<IPostRepository, EfPostRepository>();
builder.Services.AddScoped<IArticleRepository, EfArticleRepository>();
builder.Services.AddScoped<ICategoryRepository, EfCategoryRepository>();
builder.Services.AddScoped<IAboutRepository, EfAboutRepository>();

var app = builder.Build();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

await SeedData.TestVerileriniDoldur(app);



app.MapControllerRoute(
    name: "category_posts", 
    pattern: "posts/category/{id}",
    defaults: new { controller = "Posts", action = "Category" }
);


app.MapControllerRoute(
name: "post_details",
pattern: "posts/details/{url}",
defaults: new { controller = "Posts", action = "Details" }
);


app.MapControllerRoute(
    name: "home_details",
    pattern: "home/details/{url}",
    defaults: new { controller = "Home", action = "Details" }
);


app.MapControllerRoute(
    name: "defoult",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);



using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await ApplicationDbInitializer.SeedRolesAndAdmin(services);
}

app.Run();