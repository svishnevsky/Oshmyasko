using System;
using System.ComponentModel.DataAnnotations;

namespace Oshmyasko.Clients.Web.Models.Report
{
    public class ClientReportViewModel
    {
        [Display(Name = "Продукт")]
        public string ProductName { get; set; }

        [Display(Name = "Клиент")]
        public string ClientName { get; set; }

        [Display(Name = "Количество")]
        public double Count { get; set; }

        [Display(Name = "Дата")]
        public DateTime Date { get; set; }
    }
}