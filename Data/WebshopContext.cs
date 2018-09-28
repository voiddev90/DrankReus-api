using System;
using Microsoft.EntityFrameworkCore;
using DrankReus_api.Models;

namespace DrankReus_api.Data
{
  public class WebshopContext : DbContext
  {
    public WebshopContext(DbContextOptions<WebshopContext> options) : base(options) {}

    public DbSet<User> Users;
    
  }
}