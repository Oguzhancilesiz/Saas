using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saas.Domain.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Infrastructure.Persistence.Configurations
{
    public sealed class PlanFeatureLimitConfig : IEntityTypeConfiguration<PlanFeatureLimit>
    {
        public void Configure(EntityTypeBuilder<PlanFeatureLimit> b)
        {
            b.ToTable("PlanFeatureLimits");
            b.HasKey(x => x.Id);
            b.Property(x => x.PlanId).IsRequired();
            b.Property(x => x.FeatureId).IsRequired();
            b.Property(x => x.LimitPerPeriod).IsRequired();
            b.Property(x => x.Period).IsRequired();
            b.HasIndex(x => new { x.PlanId, x.FeatureId }).IsUnique();
        }
    }

}
