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
                    string response = fullname;
                    return Json(response);
                }
                else
                {
                    return Problem("Неправильний логін або пароль");
                }
            }
            return Problem("Помилка під час відправки даних");
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                var isSuccededRegister = await repo.TryToRegister(registerVM);

                return Json(isSuccededRegister);
            }
            else { return Json(false); }
        }
        [HttpPost]
        public async Task<IActionResult> TryAddReview(UserReviewVM userReviewVM)
        {
            if (userReviewVM.ReviewText != null)
            {
                //UserReviewVM userReviewVM = new() { ReviewText = ReviewText, UserLogin = Login, ReviewDate = DateTime.Now };
                userReviewVM.ReviewDate = DateTime.Now;
                await repo.CreateReview(userReviewVM);
                return Json("Відгук успішно відпралено!");
            }
            else
            {
                return Problem("Відгук не може бути порожнім!");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetReviews()
        {
            var Reviews = await repo.GetReviews();
            string response = JsonConvert.SerializeObject(Reviews);
            return Json(response);
        }
    }
}