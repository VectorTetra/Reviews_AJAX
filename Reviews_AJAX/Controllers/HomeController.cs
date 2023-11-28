using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Reviews_AJAX.Models;
using Reviews_AJAX.Repos;
using System.Diagnostics;

namespace Reviews_AJAX.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository repo;
        public HomeController(IRepository repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var fullname = await repo.TryToLogin(loginVM);
                if (fullname != null)
                {
                    //Response.Cookies.Append("FullName", fullname);
                    //Response.Cookies.Append("login", loginVM.Login);
                    //HttpContext.Session.SetString("FullName", fullname);
                    //HttpContext.Session.SetString("login", loginVM.Login);

                    string response = fullname;
                    //var response = new { Fullname = fullname, Login = loginVM.Login };
                    return Json(response);
                }
                else
                {
                    return Problem("Неправильний логін або пароль");
                }
            }
            return Problem("Помилка під час відправки даних");
        }
        //public IActionResult Register()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> GetReviews()
        {
            var Reviews = await repo.GetReviews();
            string response = JsonConvert.SerializeObject(Reviews);
            return Json(response);
        }
    }
}