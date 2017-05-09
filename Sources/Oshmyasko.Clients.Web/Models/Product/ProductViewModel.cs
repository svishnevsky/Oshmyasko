using System.ComponentModel.DataAnnotations;

namespace Oshmyasko.Clients.Web.Models.Product
{
    public class ProductViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Значение обязательно.")]
        [StringLength(50, ErrorMessage = "Значение должно быть не менее {2} и не более {1} символов.", MinimumLength = 5)]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Изображение")]
        public string Image { get; set; }

        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Значение обязательно.")]
        [StringLength(2000, ErrorMessage = "Значение должно быть не менее {2} и не более {1} символов.", MinimumLength = 5)]
        [Display(Name = "Состав")]
        public string Composition { get; set; }
    }
}
