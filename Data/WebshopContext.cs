using System;
using DrankReus_api.Models;
using Microsoft.EntityFrameworkCore;

namespace DrankReus_api.Data
{
  public class WebshopContext : DbContext
  {
    public WebshopContext(DbContextOptions<WebshopContext> options) : base(options) {}

    public DbSet<Product> Product { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<Country> Country { get; set; }
  }
}