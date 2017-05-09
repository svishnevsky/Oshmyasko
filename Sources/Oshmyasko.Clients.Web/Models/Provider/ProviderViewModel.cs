using System.ComponentModel.DataAnnotations;

namespace Oshmyasko.Clients.Web.Models.Provider
{
    public class ProviderViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Значение обязательно.")]
        [StringLength(50, ErrorMessage = "Значение должно быть не менее {2} и не более {1} символов.", MinimumLength = 5)]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Значение обязательно.")]
        [StringLength(200, ErrorMessage = "Значение должно быть не менее {2} и не более {1} символов.", MinimumLength = 5)]
        [Display(Name = "Адрес")]
        public string Address { get; set; }
        
        [Required(ErrorMessage = "Значение обязательно.")]
        [StringLength(200, ErrorMessage = "Значение должно быть не менее {2} и не более {1} символов.", MinimumLength = 5)]
        [Display(Name = "Контакты")]
        public string Contact { get; set; }
    }
}
