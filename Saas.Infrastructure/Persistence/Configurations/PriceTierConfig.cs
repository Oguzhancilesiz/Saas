using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saas.Domain.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Infrastructure.Persistence.Configurations
{
    public sealed class PriceTierConfig : IEntityTypeConfiguration<PriceTier>
    {
        public void Configure(EntityTypeBuilder<PriceTier> b)
        {
            b.ToTable("PriceTiers");
            b.HasKey(x => x.Id);
            b.Property(x => x.FromQuantity).IsRequired();
            b.Property(x => x.ToQuantity).IsRequired();
            b.Property(x => x.UnitPrice).HasColumnType("decimal(18,4)").IsRequired();
            b.HasIndex(x => new { x.PlanPriceId, x.AppPlanPriceId, x.FromQuantity, x.ToQuantity });
        }
    }

}
