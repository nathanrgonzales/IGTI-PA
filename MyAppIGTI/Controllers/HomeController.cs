using Microsoft.AspNetCore.Mvc;
using MyAppIGTI.Models;
using System.Diagnostics;

namespace MyAppIGTI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            HomeModel oHome = new HomeModel();
            oHome.UserName = "Nathan Gonzales";
            oHome.UserEmail = "nathanrgonzales@hotmail.com";
            return View(oHome);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}