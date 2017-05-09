using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Oshmyasko.Clients.Web.Models;
using Oshmyasko.Clients.Web.Models.Category;
using Oshmyasko.Clients.Web.Models.Product;
using System.Linq;

namespace Oshmyasko.Clients.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            base.ChangeTracker.AutoDetectChangesEnabled = false;
            //base.Database.EnsureCreated();
            if (base.Database.GetPendingMigrations().Any())
            {
                base.Database.Migrate();
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Category>()
                .ToTable("Categories")
                .Property(x => x.Id)
                .IsRequired()
                .UseSqlServerIdentityColumn();
            builder.Entity<Category>()
                .HasKey(x => x.Id)
                .ForSqlServerHasName("CategoryId");
            builder.Entity<Category>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode();
            builder.Entity<Category>()
                .Property(x => x.Image)
                .IsRequired()
                .HasMaxLength(1000)
                .IsUnicode();
            
            builder.Entity<Product>()
                .ToTable("Products")
                .Property(x => x.Id)
                .IsRequired()
                .UseSqlServerIdentityColumn();
            builder.Entity<Product>()
                .HasKey(x => x.Id)
                .ForSqlServerHasName("ProductId");
            builder.Entity<Product>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode();
            builder.Entity<Product>()
                .Property(x => x.Image)
                .IsRequired()
                .HasMaxLength(1000)
                .IsUnicode();
            builder.Entity<Product>()
                .Property(x => x.Composition)
                .IsRequired()
                .HasMaxLength(2000)
                .IsUnicode();
            builder.Entity<Product>()
                .HasOne<Category>()
                .WithMany()
                .HasForeignKey("CategoryId")
                .IsRequired()
                .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Cascade);
        }
    }
}
