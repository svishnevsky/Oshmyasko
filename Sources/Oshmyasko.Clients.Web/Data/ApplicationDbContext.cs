using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Oshmyasko.Clients.Web.Models;
using Oshmyasko.Clients.Web.Models.Category;
using Oshmyasko.Clients.Web.Models.Order;
using Oshmyasko.Clients.Web.Models.Product;
using Oshmyasko.Clients.Web.Models.Provider;
using System;
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

            builder.Entity<Provider>()
                .ToTable("Providers")
                .Property(x => x.Id)
                .IsRequired()
                .UseSqlServerIdentityColumn();
            builder.Entity<Provider>()
                .HasKey(x => x.Id)
                .ForSqlServerHasName("ProviderId");
            builder.Entity<Provider>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode();
            builder.Entity<Provider>()
                .Property(x => x.Address)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode();
            builder.Entity<Provider>()
                .Property(x => x.Contact)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode();

            builder.Entity<Order>()
                .ToTable("Orders")
                .Property(x => x.Id)
                .IsRequired()
                .UseSqlServerIdentityColumn();
            builder.Entity<Order>()
                .HasKey(x => x.Id)
                .ForSqlServerHasName("OrderId");
            builder.Entity<Order>()
                .Property(x => x.Count)
                .IsRequired();
            builder.Entity<Order>()
                .Property(x => x.Confirmed)
                .IsRequired(false);
            builder.Entity<Order>()
                .Property(x => x.Client)
                .IsRequired();
            builder.Entity<Order>()
                .Property(x => x.Created)
                .ForSqlServerHasDefaultValueSql("GETDATE()")
                .IsRequired();
            builder.Entity<Order>()
                .HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .IsRequired()
                .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Cascade);
            builder.Entity<Order>()
                .HasOne(x => x.ClientInfo)
                .WithMany()
                .HasForeignKey(x => x.Client)
                .HasPrincipalKey("UserName")
                .IsRequired()
                .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Cascade);
        }
    }
}
