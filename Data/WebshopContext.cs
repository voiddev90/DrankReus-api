using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DrankReus_api.Models;
using Microsoft.EntityFrameworkCore;

namespace DrankReus_api.Data
{
  public class WebshopContext : DbContext
  {
    public WebshopContext(DbContextOptions<WebshopContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<Country> Country { get; set; }
    public DbSet<Brand> Brand { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public DbSet<Whishlist> Whishlists { get; set; }

    protected override void OnModelCreating(ModelBuilder ModelBuilder)
    {
      ModelBuilder
      .Entity<Order>()
      .Property(o => o.OrderStatus)
      .HasConversion(
        v => v.ToString(),
        v => (OrderStatusEnum)Enum.Parse(typeof(OrderStatusEnum), v)
      );

      ModelBuilder.Entity<User>()
      .Property(u => u.DiscountPoints)
      .HasDefaultValue(0);

      ModelBuilder.Entity<Order>()
      .Property(o => o.DiscountPercentage)
      .HasDefaultValue(0);
    }

  }
}
