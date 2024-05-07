﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities.Product;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Infrastructure._Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext dbContext)
        {
            if (!dbContext.ProductBrands.Any())
            {
                var brandData = File.ReadAllText("../Talabat.Infrastructure/_Data/DataSeed/brands.json");

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
                var categoriesData = File.ReadAllText("../Talabat.Infrastructure/_Data/DataSeed/categories.json");

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

                var productsData = File.ReadAllText("../Talabat.Infrastructure/_Data/DataSeed/products.json");
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

			if (!dbContext.DelivreyMethods.Any())
			{
				var deliveryMethodJson = File.ReadAllText("../Talabat.Infrastructure/_Data/DataSeed/delivery.json");
				var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodJson);

				if (deliveryMethods?.Count > 0)
				{
					foreach (var item in deliveryMethods)
					{
						dbContext.DelivreyMethods.Add(item);
					}
				}
				await dbContext.SaveChangesAsync();
			}
		}
    }
}