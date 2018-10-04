using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DrankReus_api.Controllers;
using DrankReus_api.Data;
using DrankReus_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DrankReus_api
{
    public class SeedDatabase
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (StreamReader d = new StreamReader(Directory.GetCurrentDirectory() + "/SeedData/Drank.json"))
            {
                var db = serviceProvider.GetRequiredService<WebshopContext>();
                var data = JObject.Parse(d.ReadToEnd());
                foreach (var entry in data.SelectToken("$.Country"))
                {
                    Country temp = new Country{Name = (string) entry["Name"]};
                    db.Add(temp);
                }
                
                foreach (var entry in data.SelectToken("$.Category"))
                {
                    Category temp = new Category{Name = (string) entry["Name"]};
                    db.Add(temp);
                }
                foreach (var entry in data.SelectToken("$.Brand"))
                {
                    Brand temp = new Brand{Name = (string) entry["Name"]};
                    db.Add(temp);
                }
                db.SaveChanges();
                foreach (var entry in data.SelectToken("$.Drank"))
                {
                    Product prod = new Product
                    {
                        Alcoholpercentage = (decimal)entry["Alcoholpercentage"],
                        Price = (decimal)entry["Price"],
                        Name = (string)entry["Name"],
                        Url = (string)entry["Url"],
                        Description = (string)entry["Description"],
                        Volume = (string)entry["Volume"],
                        CountryEntity= db.Country.Select(x => x).FirstOrDefault(x => x.Name == (string)entry["Country"]),
                        CategoryEntity = db.Category.Select(x => x).FirstOrDefault(x => x.Name == (string)entry["Category"]),
                        BrandEntity = db.Brand.Select(x => x).FirstOrDefault(x => x.Name == (string)entry["Brand"])
                        
                        
                    };
                    db.Add(prod);
                }
                db.SaveChanges();
            }
            
        }

        /*public static void Initialize(IServiceProvider serviceProvider)
        {
            var db = serviceProvider.GetRequiredService<WebshopContext>();
            //BuildTable<Product>(Directory.GetCurrentDirectory() + "/SeedData/Drank.json", db);
            using (StreamReader d = new StreamReader(Directory.GetCurrentDirectory() + "/SeedData/Drank.json"))
            using (StreamReader b = new StreamReader(Directory.GetCurrentDirectory() + "/SeedData/DrankBrand.json")) 
            using (StreamReader co = new StreamReader(Directory.GetCurrentDirectory() + "/SeedData/DrankCountry.json"))
            using (StreamReader ca = new StreamReader(Directory.GetCurrentDirectory() + "/SeedData/DrankCategory.json"))
            {
                if (!db.Product.Any())
                {
                    string djson = d.ReadToEnd();
                    string bjson = b.ReadToEnd();
                    string cojson = co.ReadToEnd();
                    string cajson = ca.ReadToEnd();
                    List<Product> drank = JsonConvert.DeserializeObject<List<Product>>(djson);
                    List<Category> category = JsonConvert.DeserializeObject<List<Category>>(cajson);
                    List<Country> country = JsonConvert.DeserializeObject<List<Country>>(cojson);
                    List<Brand> brand = JsonConvert.DeserializeObject<List<Brand>>(bjson);
                    foreach (var entry in brand)
                    {
                        Brand productBrand = new Brand
                        {
                            Name = entry.Name
                        };
                        db.Add(productBrand);
                    }
                    db.SaveChanges();
                    foreach (var entry in category)
                    {
                        Category productCategory = new Category
                        {
                            Name = entry.Name
                        };
                        db.Add(productCategory);
                    }
                    db.SaveChanges();
                    foreach (var entry in country)
                    {
                        Country productCountry = new Country
                        {
                            Name = entry.Name
                        };
                        db.Add(productCountry);
                    }
                    db.SaveChanges();
                    foreach (var drink in drank)
                    {
                        Product prod = new Product
                        {
                            Alcoholpercentage = drink.Alcoholpercentage,
                            Description = drink.Description,
                            Name = drink.Name,
                            Price = drink.Price,
                            Url = drink.Url,
                            Volume = drink.Volume,
                            CountryEntity =
                                db.Country.Where(n => n.Name == drink.Country).Select(x => x).FirstOrDefault(),
                            CategoryEntity = db.Category.Where(n => n.Name == drink.Category).Select(x => x)
                                .FirstOrDefault(),
                            BrandEntity = db.Brand.Where(n => n.Name == drink.Brand).Select(x => x)
                                .FirstOrDefault()
                        };
                        db.Add(prod);
                    }
                    db.SaveChanges();
                    db.Dispose();
                }
            }
            
        }*/
    }
}
