using System.ComponentModel.DataAnnotations;

namespace Reviews_AJAX.Models
{
    // клас моделі-представлення (view-model)
    public class LoginVM
    {
        [Required]
        [Display(Name = "Логін")]
        public string? Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }
    }
}
