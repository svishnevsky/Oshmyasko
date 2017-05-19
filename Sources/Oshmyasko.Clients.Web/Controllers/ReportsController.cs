using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Oshmyasko.Clients.Web.Data;
using Oshmyasko.Clients.Web.Models;
using Oshmyasko.Clients.Web.Models.Category;
using Oshmyasko.Clients.Web.Models.Order;
using Oshmyasko.Clients.Web.Models.Product;
using Oshmyasko.Clients.Web.Models.Report;
using System;
using System.Linq;

namespace Oshmyasko.Clients.Web.Controllers
{
    [Authorize(Roles = "Сотрудник")]
    public class ReportsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private ApplicationDbContext context;

        public ReportsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Product(int? id, DateTime? from, DateTime? to)
        {
            var model = this.context.Set<Order>()
                .Where(x => x.Confirmed == true && (!id.HasValue || x.ProductId == id.Value) && (!from.HasValue || x.Created >= from.Value) && (!to.HasValue || x.Created <= to.Value))
                .Select(x => new ReportViewModel
                {
                    Count = x.Count,
                    Date = x.Created,
                    Name = x.Product.Name
                })
                .ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Client(string client, DateTime? from, DateTime? to)
        {
            var model = this.context.Set<Order>()
                .Where(x => x.Confirmed == true && (string.IsNullOrEmpty(client) || x.Client == client) && (!from.HasValue || x.Created >= from.Value) && (!to.HasValue || x.Created <= to.Value))
                .Select(x => new ReportViewModel
                {
                    Count = x.Count,
                    Date = x.Created,
                    Name = x.ClientInfo.Name
                })
                .ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Warehouse(int? categoryId)
        {
            var model = this.context.Set<Product>()
                .Where(x => !categoryId.HasValue || x.CategoryId == categoryId.Value)
                .Select(x => new ReportViewModel
                {
                    Name = x.Name,
                    Count = x.Quantity ?? 0,
                    Category = new CategoryViewModel { Id = x.CategoryId, Name = x.Category.Name }
                })
                .ToList();
            return View(model);
        }
    }
}
