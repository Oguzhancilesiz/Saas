using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saas.Domain.Usage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Infrastructure.Persistence.Configurations
{
    public sealed class UsageLogConfig : IEntityTypeConfiguration<UsageLog>
    {
        public void Configure(EntityTypeBuilder<UsageLog> b)
        {
            b.ToTable("UsageLogs");
            b.HasKey(x => x.Id);

            b.Property(x => x.UserId).IsRequired();
            b.Property(x => x.AppId).IsRequired();
            b.Property(x => x.FeatureCode).IsRequired().HasMaxLength(64);
            b.Property(x => x.Quantity).HasDefaultValue(1);
            b.Property(x => x.OccurredAtUtc).IsRequired();

            b.HasIndex(x => new { x.UserId, x.AppId, x.FeatureCode, x.OccurredAtUtc });
        }
    }

}
