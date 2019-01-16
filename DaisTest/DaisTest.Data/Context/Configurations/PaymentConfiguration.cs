using DaisTest.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DaisTest.Data.Context.Configurations
{
	internal class PaymentConfiguration : IEntityTypeConfiguration<Payment>
	{
		public void Configure(EntityTypeBuilder<Payment> builder)
		{
			builder.ToTable("Payments");

			builder.HasOne(us => us.User)
				.WithMany(u => u.Payments)
				.HasForeignKey(us => us.UserId)
				.HasPrincipalKey(u => u.Id);
		}
	}
}
