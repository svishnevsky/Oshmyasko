using System.ComponentModel.DataAnnotations;

namespace Oshmyasko.Clients.Web.Models.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Значение обязательно.")]
        [StringLength(30, ErrorMessage = "Значение должно быть не менее {2} и не более {1} символов.", MinimumLength = 5)]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Значение обязательно.")]
        [StringLength(100, ErrorMessage = "Значение должно быть не менее {2} и не более {1} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
