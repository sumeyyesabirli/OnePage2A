using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnePage2ABussiness.AboutUs.Abstract;
using OnePage2ABussiness.AboutUs.Concrete;
using OnePage2ABussiness.Contacts.Abstract;
using OnePage2ABussiness.Contacts.Concrete;
using OnePage2ABussiness.Gallery.Abstract;
using OnePage2ABussiness.Login.Abstract;
using OnePage2ABussiness.References.Abstract;
using OnePage2ABussiness.References.Concrete;
using OnePage2ABussiness.Register.Abstract;
using OnePage2ABussiness.Register.Concrete;
using OnePage2ABussiness.Services.Abstract;
using OnePage2ABussiness.Services.Concrete;
using OnePage2ABussiness.Users.Abstract;
using OnePage2ABussiness.Users.Concrete;
using OnePage2AClientBussines.Banners.Abstract;
using OnePage2ACore.Abstract;
using OnePage2ACore.Concrete;
using OnePage2ADataAccess.Contexts;
using OnePage2ADataAccess.Repositories.Abstract;
using OnePage2ADataAccess.Repositories.Concrete;
using OnePage2AEntity.Entites;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

//HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Configure DbContext
builder.Services.AddDbContext<DbContext2A>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add Generic Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Add Services
builder.Services.AddScoped<IBannerService, BannerService>();
builder.Services.AddScoped<IAboutUsServices, AboutUsServices>();
builder.Services.AddScoped<IContactServices, ContactServices>();
builder.Services.AddScoped<IReferencesServices, ReferencesServices>();
builder.Services.AddScoped<IServiceServices, ServiceServices>();
builder.Services.AddScoped<ILoginServices, LoginServices>();
builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<IGalleryService, GalleryService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IFileService, FileService>();


// Identity PasswordHasher
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.LogoutPath = "/Login/Logout";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
    });

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("admin"));
    options.AddPolicy("RequireEditorRole", policy => policy.RequireRole("admin", "editor"));
});

var app = builder.Build();

// Error Handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Route Mapping
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seed Admin User
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DbContext2A>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

    if (!context.Users.Any())
    {
        var adminUser = new User
        {
            Name = "admin",
            Email = "admin@example.com",
            Role = "admin",
            CreatedByName = "System",
            IsActive = true
        };
        adminUser.Password = passwordHasher.HashPassword(adminUser, "Admin@123");
        context.Users.Add(adminUser);
        await context.SaveChangesAsync();
    }
}

app.Run();
