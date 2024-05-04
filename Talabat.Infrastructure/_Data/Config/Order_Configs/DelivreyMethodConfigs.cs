using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entitites.Order_Aggregate;

namespace Talabat.Infrastructure._Data.Config.Order_Configs
{
	internal class DelivreyMethodConfigs : IEntityTypeConfiguration<DeliveyMethod>
	{
		public void Configure(EntityTypeBuilder<DeliveyMethod> builder)
		{
			builder.Property(deliveryMethod => deliveryMethod.Cost).HasColumnType("decimal(12,2)");
		}
	}
}
