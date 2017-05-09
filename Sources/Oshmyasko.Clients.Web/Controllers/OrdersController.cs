using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oshmyasko.Clients.Web.Data;
using Oshmyasko.Clients.Web.Models;
using Oshmyasko.Clients.Web.Models.Order;
using Oshmyasko.Clients.Web.Models.Product;
using System.Linq;
using System.Threading.Tasks;

namespace Oshmyasko.Clients.Web.Controllers
{
    [Authorize(Roles = "Клиент")]
    public class OrdersController : Controller
    {
        private ApplicationDbContext context;

        public OrdersController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult List(string client = null)
        {
            var model = this.context.Set<Order>()
                .Where(x => string.IsNullOrEmpty(client) ? true : x.Client == client)
                .Select(x => new OrderViewModel
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    Count = x.Count,
                    Confirmed = x.Confirmed,
                    Created = x.Created,
                    Product = new ProductViewModel { Id = x.Product.Id, CategoryId = x.Product.CategoryId, Name = x.Product.Name },
                    ClientInfo = new ApplicationUser { Name = x.ClientInfo.Name }
                })
                .OrderByDescending(x => x.Created)
                .ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Post(OrderViewModel model, string client)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var entity = new Order
            {
                Count = model.Count,
                ProductId = model.ProductId,
                Client = client
            };

            //if (model.Id.HasValue)
            //{
            //    var entry = this.context.Attach(entity);
            //TODO: Created
            //    entry.State = EntityState.Modified;
            //}
            //else
            //{
                this.context.Add(entity);
            //}

            await this.context.SaveChangesAsync();
            return RedirectToAction("List");
        }
    }
}
