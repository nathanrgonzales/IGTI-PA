using MyAppIGTI.Data;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using MyAppIGTI.DBRepo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEntityFrameworkSqlServer().AddDbContext<DBMyAppContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));
builder.Services.AddScoped<IProfileTestRepo, ProfileTestRepo>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
