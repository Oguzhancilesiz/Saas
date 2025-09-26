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
    public sealed class AppPlanPriceConfig : IEntityTypeConfiguration<AppPlanPrice>
    {
        public void Configure(EntityTypeBuilder<AppPlanPrice> b)
        {
            b.ToTable("AppPlanPrices");
            b.HasKey(x => x.Id);
            b.Property(x => x.AppId).IsRequired();
            b.Property(x => x.PlanId).IsRequired();
            b.Property(x => x.Currency).IsRequired().HasMaxLength(8);
            b.Property(x => x.BillingPeriod).IsRequired();
            b.Property(x => x.Model).IsRequired();
            b.Property(x => x.Amount).HasColumnType("decimal(18,2)");
            b.HasIndex(x => new { x.AppId, x.PlanId, x.BillingPeriod, x.EffectiveFromUtc, x.EffectiveToUtc });
        }
    }
}
