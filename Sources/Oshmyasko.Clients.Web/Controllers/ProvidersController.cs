using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oshmyasko.Clients.Web.Data;
using Oshmyasko.Clients.Web.Models.Provider;
using System.Linq;
using System.Threading.Tasks;

namespace Oshmyasko.Clients.Web.Controllers
{
    [Authorize(Roles = "Сотрудник")]
    public class ProvidersController : Controller
    {
        private ApplicationDbContext context;

        public ProvidersController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult List()
        {
            var model = this.context.Set<Provider>()
                .Select(x => new ProviderViewModel { Id = x.Id, Address = x.Address, Name = x.Name, Contact = x.Contact })
                .ToList();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id = null)
        {
            ProviderViewModel model = null;
            if (id.HasValue)
            {
                var provider = await this.context.Set<Provider>().FirstOrDefaultAsync(x => x.Id == id.Value);
                if (provider == null)
                {
                    return NotFound();
                }

                model = new ProviderViewModel { Id = provider.Id, Address = provider.Address, Name = provider.Name, Contact = provider.Contact };
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProviderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var entity = new Provider
            {
                Id = model.Id,
                Address = model.Address,
                Name = model.Name,
                Contact = model.Contact
            };

            if (model.Id.HasValue)
            {
                var entry = this.context.Attach(entity);
                entry.State = EntityState.Modified;
            }
            else
            {
                this.context.Add(entity);
            }

            await this.context.SaveChangesAsync();
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await this.context.Set<Provider>().FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            this.context.Remove(entity);
            await this.context.SaveChangesAsync();
            return RedirectToAction("List");
        }
    }
}
