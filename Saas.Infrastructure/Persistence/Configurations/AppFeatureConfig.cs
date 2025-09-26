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
    public sealed class AppFeatureConfig : IEntityTypeConfiguration<AppFeature>
    {
        public void Configure(EntityTypeBuilder<AppFeature> b)
        {
            b.ToTable("AppFeatures");
            b.HasKey(x => x.Id);
            b.Property(x => x.AppId).IsRequired();
            b.Property(x => x.FeatureId).IsRequired();
            b.Property(x => x.IsEnabled).HasDefaultValue(true);
            b.HasIndex(x => new { x.AppId, x.FeatureId }).IsUnique();
        }
    }

}
