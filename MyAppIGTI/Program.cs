using MyAppIGTI.Data;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using MyAppIGTI.DBRepo;
using static NuGet.Packaging.PackagingConstants;
using MyAppIGTI.AppVariable;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ConfigureEndpointDefaults(listenOptions =>
    {
        // ...
    });
});

// Add services to the container.
builder.Services.Configure<AppVariables>(builder.Configuration.GetSection("AppVariables"));
builder.Services.AddControllersWithViews();
builder.Services.AddEntityFrameworkSqlServer().AddDbContext<DBMyAppContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));
builder.Services.AddScoped<IProfileTestRepo, ProfileTestRepo>();
builder.Services.AddOptions();

var omainPath = builder.Configuration.GetSection("AppVariables")["MainPath"];

if (string.IsNullOrEmpty(omainPath))
{
    throw new Exception("Cannot read the maind folder path");
}

if (!Directory.Exists(omainPath))
{
    try
    {
        Directory.CreateDirectory(omainPath);
    }
    catch 
    {
        throw new Exception("Cannot create the main folder");
    }
}

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
