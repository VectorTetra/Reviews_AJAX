using Reviews_AJAX.Models;
using Reviews_AJAX.Repos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Отримуємо рядок підключення з конфігураційного файлу
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// додаємо контекст ApplicationContext як сервіс у додаток
builder.Services.AddDbContext<ReviewContext>(options => options.UseSqlServer(connection));
builder.Services.AddDistributedMemoryCache();
// додаємо залежність від репозиторію, для опосередкованого керування БД
builder.Services.AddScoped<IRepository, ReviewsRepository>();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(opt =>
{
    opt.IdleTimeout = TimeSpan.FromMinutes(30);
    opt.Cookie.Name = "Session";
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
