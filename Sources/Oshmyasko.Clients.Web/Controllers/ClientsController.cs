using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Oshmyasko.Clients.Web.Data;
using Oshmyasko.Clients.Web.Models;
using Oshmyasko.Clients.Web.Models.Client;
using Oshmyasko.Clients.Web.Models.Order;
using System.Linq;
using System.Threading.Tasks;

namespace Oshmyasko.Clients.Web.Controllers
{
    [Authorize(Roles = "Сотрудник")]
    public class ClientsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private ApplicationDbContext context;

        public ClientsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> List(string client = null)
        {
            var clients = await this.userManager.GetUsersInRoleAsync("Клиент");
            var model = clients.Select(x => new ClientViewModel
            {
                Name = x.Name,
                Address = x.Address,
                Contact = x.Contact,
                UserName = x.UserName,
                Orders = this.context.Set<Order>().Count(o => o.Client == x.UserName)
            });

            return View(model);
        }
    }
}
