﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oshmyasko.Clients.Web.Data;
using Oshmyasko.Clients.Web.Models.Product;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Oshmyasko.Clients.Web.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private IHostingEnvironment environment;
        private ApplicationDbContext context;

        public ProductsController(IHostingEnvironment environment, ApplicationDbContext context)
        {
            this.environment = environment;
            this.context = context;
        }

        [HttpGet]
        public IActionResult List(int? categoryId)
        {
            var model = this.context.Set<Product>()
                .Where(x => !categoryId.HasValue || x.CategoryId == categoryId.Value)
                .Select(x => new ProductViewModel { Id = x.Id, Image = x.Image, Name = x.Name, CategoryId = x.CategoryId, Composition = x.Composition, Quantity = x.Quantity ?? 0 })
                .ToList();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            ProductViewModel model = null;
            var product = await this.context.Set<Product>().FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            model = new ProductViewModel { Id = product.Id, Image = product.Image, Name = product.Name, CategoryId = product.CategoryId, Composition = product.Composition, Quantity = product.Quantity ?? 0 };
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Сотрудник")]
        public async Task<IActionResult> Edit(int? id = null)
        {
            ProductViewModel model = null;
            if (id.HasValue)
            {
                var product = await this.context.Set<Product>().FirstOrDefaultAsync(x => x.Id == id.Value);
                if (product == null)
                {
                    return NotFound();
                }

                model = new ProductViewModel { Id = product.Id, Image = product.Image, Name = product.Name, CategoryId = product.CategoryId, Composition = product.Composition, Quantity = product.Quantity ?? 0 };
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Сотрудник")]
        public async Task<IActionResult> Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (Request.Form.Files?.Count > 0)
            {
                var file = Request.Form.Files[0];
                if (file.ContentType != "application/octet-stream")
                {
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
            }

            var entity = new Product
            {
                Id = model.Id,
                Image = model.Image,
                Name = model.Name,
                Composition = model.Composition,
                CategoryId = model.CategoryId,
                Quantity = model.Quantity
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
            var entity = await this.context.Set<Product>().FirstOrDefaultAsync(x => x.Id == id);
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
