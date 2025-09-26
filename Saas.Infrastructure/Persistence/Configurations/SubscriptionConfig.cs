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
    public sealed class SubscriptionConfig : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> b)
        {
            b.ToTable("Subscriptions");
            b.HasKey(x => x.Id);

            b.Property(x => x.UserId).IsRequired();
            b.Property(x => x.AppId).IsRequired();
            b.Property(x => x.PlanId).IsRequired();

            b.Property(x => x.StartUtc).IsRequired();
            b.Property(x => x.EndUtc).IsRequired();

            b.Property(x => x.Status).IsRequired();

            b.HasIndex(x => new { x.UserId, x.AppId, x.Status });

            // İleride App/Plan FK’lerini Domain’e ekleyince navigation’lar bağlanabilir
            // Şimdilik guid alanlarıyla gidiyoruz.
        }
    }

}
