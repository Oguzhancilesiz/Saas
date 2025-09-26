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
    public sealed class AppPlanConfig : IEntityTypeConfiguration<AppPlan>
    {
        public void Configure(EntityTypeBuilder<AppPlan> b)
        {
            b.ToTable("AppPlans");
            b.HasKey(x => x.Id);
            b.Property(x => x.AppId).IsRequired();
            b.Property(x => x.PlanId).IsRequired();
            b.Property(x => x.IsEnabled).HasDefaultValue(true);
            b.HasIndex(x => new { x.AppId, x.PlanId }).IsUnique();
        }
    }

}
