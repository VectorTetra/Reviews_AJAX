using Microsoft.AspNetCore.Mvc;
using Reviews_AJAX.Models;
using System.Diagnostics;

namespace Reviews_AJAX.Controllers
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
            return View();
        }
    }
}