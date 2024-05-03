using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Product;

namespace Talabat.Infrastructure._Data
{
    public class StoreContext : DbContext
    {
        private readonly DbContextOptions<StoreContext> _options;

        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
            _options = options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        =>
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
    }
}
