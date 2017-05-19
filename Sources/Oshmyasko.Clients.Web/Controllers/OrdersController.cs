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
    public class OrdersController : Controller
    {
        private ApplicationDbContext context;

        public OrdersController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Клиент,Сотрудник")]
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
                    Client = new UserViewModel { Name = x.ClientInfo.Name, UserName = x.ClientInfo.UserName }
                })
                .OrderByDescending(x => x.Created)
                .ToList();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Клиент")]
        public async Task<IActionResult> Post(OrderViewModel model, string client)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Get", "Products", new { id = model.ProductId });
            }

            var product = await this.context.Set<Product>().FirstOrDefaultAsync(x => x.Id == model.ProductId);
            if (product == null)
            {
                return NotFound();
            }

            if (model.Count > product.Quantity)
            {
                ModelState.AddModelError(nameof(OrderViewModel.Count), "На складе недостаточно продукции.");
                return RedirectToAction("Get", "Products", new { id = model.ProductId });
            }

            product.Quantity -= model.Count;
            var productEntry = this.context.Attach(product);
            productEntry.State = EntityState.Modified;
            var entity = new Order
            {
                Count = model.Count,
                ProductId = model.ProductId,
                Client = client
            };

            this.context.Add(entity);
            await this.context.SaveChangesAsync();
            return RedirectToAction("List");
        }

        [HttpPost]
        [Authorize(Roles = "Сотрудник")]
        public async Task<IActionResult> Put(string client, int id, bool confirmed)
        {
            var entity = await this.context.Set<Order>().FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            entity.Confirmed = confirmed;
            var entry = this.context.Attach(entity);
            entry.State = EntityState.Modified;
            await this.context.SaveChangesAsync();
            return RedirectToAction("List");
        }
    }
}
