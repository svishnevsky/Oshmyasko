using System;
using Oshmyasko.Clients.Web.Models.Category;

namespace Oshmyasko.Clients.Web.Models.Report
{
    public class ReportViewModel
    {
        public string Name { get; set; }

        public double Count { get; set; }

        public DateTime Date { get; set; }

        public CategoryViewModel Category { get; internal set; }
    }
}
