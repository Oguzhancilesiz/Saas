using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saas.Domain.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Infrastructure.Persistence.Configurations
{
    public sealed class PlanConfig : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> b)
        {
            b.ToTable("Plans");
            b.HasKey(x => x.Id);

            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(120);

            b.Property(x => x.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            b.Property(x => x.DurationDays)
                .IsRequired();

            b.Property(x => x.Status)
                .IsRequired();

            b.HasIndex(x => x.Name).IsUnique(false);
        }
    }

}
