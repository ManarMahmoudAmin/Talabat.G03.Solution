using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Infrastructure._Data.Config.Order_Configs
{
	internal class OrderConfigurations : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.OwnsOne(order => order.ShippingAddress, shippingAddress => shippingAddress.WithOwner());

			builder.Property(order => order.Status)
				.HasConversion
				(
					(orderStatus) => orderStatus.ToString(),
					(orderStatus) => (OrderStatus)Enum.Parse(typeof(OrderStatus), orderStatus)
				);
			builder.Property(order => order.SubTotal).HasColumnType("decimal(12,2)");

			builder.HasOne(order => order.DeliveyMethod).WithMany().OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(order => order.Items).WithOne().OnDelete(DeleteBehavior.Cascade);

		}
	}
}
