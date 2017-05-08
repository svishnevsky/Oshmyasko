using System.ComponentModel.DataAnnotations;

namespace Oshmyasko.Clients.Web.Models.Category
{
    public class CategoryViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Значение обязательно.")]
        [StringLength(30, ErrorMessage = "Значение должно быть не менее {2} и не более {1} символов.", MinimumLength = 5)]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Изображение")]
        public string Image { get; set; }
    }
}