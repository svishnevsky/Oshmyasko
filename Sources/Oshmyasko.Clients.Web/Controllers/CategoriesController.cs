using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oshmyasko.Clients.Web.Data;
using Oshmyasko.Clients.Web.Models.Category;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Oshmyasko.Clients.Web.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private IHostingEnvironment environment;
        private ApplicationDbContext context;

        public CategoriesController(IHostingEnvironment environment, ApplicationDbContext context)
        {
            this.environment = environment;
            this.context = context;
        }

        [HttpGet]
        public IActionResult List()
        {
            var model = this.context.Set<Category>()
                .Select(x => new CategoryViewModel { Id = x.Id, Image = x.Image, Name = x.Name })
                .ToList();
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Сотрудник")]
        public async Task<IActionResult> Edit(int? id = null)
        {
            CategoryViewModel model = null;
            if (id.HasValue)
            {
                var category = await this.context.Set<Category>().FirstOrDefaultAsync(x => x.Id == id.Value);
                if (category == null)
                {
                    return NotFound();
                }

                model = new CategoryViewModel { Id = category.Id, Image = category.Image, Name = category.Name };
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Сотрудник")]
        public async Task<IActionResult> Edit(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (Request.Form.Files?.Count > 0)
            {
                var file = Request.Form.Files[0];
                if (!file.ContentType.StartsWith("image/"))
                {
                    ModelState.AddModelError(string.Empty, "Загружать можно только изображения.");
                    return View(model);
                }

                var uploadsPath = Path.Combine(this.environment.WebRootPath, "Uploads");
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                var fileName = $"{Guid.NewGuid().ToString("n")}{Path.GetExtension(file.FileName)}";
                using (var fileStream = new FileStream(Path.Combine(uploadsPath, fileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                model.Image = $"~/Uploads/{fileName}";
            }

            var entity = new Category
            {
                Id = model.Id,
                Image = model.Image,
                Name = model.Name
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
        [Authorize(Roles = "Сотрудник")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await this.context.Set<Category>().FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return NotFound();
            }

            this.context.Set<Category>().Remove(entity);
            await this.context.SaveChangesAsync();
            return RedirectToAction("List");
        }
    }
}
