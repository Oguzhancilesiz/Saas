using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saas.Domain.Apps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Infrastructure.Persistence.Configurations
{
    public class AppConfig : IEntityTypeConfiguration<App>
    {
        public void Configure(EntityTypeBuilder<App> b)
        {
            b.ToTable("Apps");
            b.HasKey(x => x.Id);
            b.Property(x => x.Name).IsRequired().HasMaxLength(100);
            b.Property(x => x.Code).IsRequired().HasMaxLength(32);
            b.HasIndex(x => x.Code).IsUnique();
        }
    }

}
