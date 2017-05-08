using System.ComponentModel.DataAnnotations;

namespace Oshmyasko.Clients.Web.Models.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Поле обязательно.")]
        [StringLength(30, ErrorMessage = "Поле должно быть не менее {2} и не более {1} символов.", MinimumLength = 5)]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Поле обязательно.")]
        [StringLength(30, ErrorMessage = "Поле должно быть не менее {2} и не более {1} символов.", MinimumLength = 2)]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Поле обязательно.")]
        [StringLength(30, ErrorMessage = "Поле должно быть не менее {2} и не более {1} символов.", MinimumLength = 2)]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Поле обязательно.")]
        [StringLength(30, ErrorMessage = "Поле должно быть не менее {2} и не более {1} символов.", MinimumLength = 2)]
        [Display(Name = "Отчество")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Поле обязательно.")]
        [Display(Name = "Роль")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Поле обязательно.")]
        [StringLength(100, ErrorMessage = "Поле должно быть не менее {2} и не более {1} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }
    }
}
