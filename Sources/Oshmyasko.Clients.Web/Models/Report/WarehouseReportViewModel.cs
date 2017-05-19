using System.ComponentModel.DataAnnotations;

namespace Oshmyasko.Clients.Web.Models.Report
{
    public class WarehouseReportViewModel
    {
        [Display(Name = "Продукт")]
        public string Name { get; set; }

        [Display(Name = "Категория")]
        public string Category { get; set; }

        [Display(Name = "Количество")]
        public double Count { get; set; }
    }
}