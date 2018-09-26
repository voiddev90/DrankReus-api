using System;
using Microsoft.EntityFrameworkCore;

namespace DrankReus_api.Data
{
  public class WebshopContext : DbContext
  {
    public WebshopContext(DbContextOptions<WebshopContext> options) : base(options) {}

    
  }
}