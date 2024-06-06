using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Entities.Product;

namespace Talabat.Infrastructure.Data
{
    public static class ApplicationContextSeed
    {
       
            public static async Task SeedAsync(ApplicationDbContext dbContext)
            {
                if (!dbContext.ProductBrands.Any())
                {
                    var brandData = File.ReadAllText("../Talabat.Infrastructure/Data/DataSeed/brands.json");

                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

                    if (brands?.Count > 0)
                    {
                        foreach (var brand in brands)
                        {
                            dbContext.Set<ProductBrand>().Add(brand);
                        }
                        await dbContext.SaveChangesAsync();
                    }
                }

               if (!dbContext.ProductCategories.Any())
               {
                   var categoriesData = File.ReadAllText("../Talabat.Infrastructure/Data/DataSeed/categories.json");

                   var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);

                   if (categories?.Count > 0)
                   {
                       foreach (var category in categories)
                       {
                           dbContext.Set<ProductCategory>().Add(category);
                       }
                       await dbContext.SaveChangesAsync();
                   }
               }

               if (!dbContext.Products.Any())
               {
                    var productsData = File.ReadAllText("../Talabat.Infrastructure/Data/DataSeed/products.json");

                   var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                   
                   if (products?.Count > 0)
                   {
                       foreach (var product in products)
                       {
                           dbContext.Set<Product>().Add(product);
                       }
                       await dbContext.SaveChangesAsync();
                }
            }

            if (!dbContext.DeliveryMethods.Any())
            {
                var deliveryMethodsData = File.ReadAllText("../Talabat.Infrastructure/Data/DataSeed/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);

                if (deliveryMethods?.Count > 0)
                {
                    foreach (var deliveryMethod in deliveryMethods)
                    {
                        dbContext.Set<DeliveryMethod>().Add(deliveryMethod);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
        }
        }
    }
