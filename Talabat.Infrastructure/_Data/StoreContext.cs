using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Reflection;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Entities.Product;
using Order = Talabat.Core.Entities.Order_Aggregate.Order;
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
        public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrdersItems { get; set; }
		public DbSet<DeliveryMethod> DelivreyMethods { get; set; }


	}
}
