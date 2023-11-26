using System.ComponentModel.DataAnnotations;

namespace Reviews_AJAX.Models
{
    // клас моделі-представлення (view-model)
    public class UserReviewVM
    {
        [Display(Name = "Користувач")]
        [Required(ErrorMessage = "Логін користувача обов'язковий для заповнення")]
        public string UserLogin { get; set; }
        [Required(ErrorMessage = "Текст відгуку обов'язковий для заповнення")]
        [Display(Name = "Відгук")]
        public string ReviewText { get; set; }
        [Display(Name = "Дата відгуку")]
        public DateTime ReviewDate { get; set; }
    }
}
