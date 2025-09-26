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
    public sealed class FeatureLimitConfig : IEntityTypeConfiguration<FeatureLimit>
    {
        public void Configure(EntityTypeBuilder<FeatureLimit> b)
        {
            b.ToTable("FeatureLimits");
            b.HasKey(x => x.Id);
            b.Property(x => x.FeatureId).IsRequired();
            b.Property(x => x.Period).IsRequired();
            b.Property(x => x.LimitPerPeriod).IsRequired();
            b.HasIndex(x => new { x.FeatureId, x.AppId, x.PlanId, x.UserId, x.Period, x.EffectiveFromUtc, x.EffectiveToUtc });
        }
    }
}
