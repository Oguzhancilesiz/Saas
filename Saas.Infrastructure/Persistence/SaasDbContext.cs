using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Saas.Domain.Apps;
using Saas.Domain.Subscriptions;
using Saas.Infrastructure.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Infrastructure.Persistence
{
    public class SaasDbContext
     : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public SaasDbContext(DbContextOptions<SaasDbContext> options) : base(options) { }

        public DbSet<App> Apps => Set<App>();
        public DbSet<Plan> Plans => Set<Plan>();
        public DbSet<Subscription> Subscriptions => Set<Subscription>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Identity tablolarını kur
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SaasDbContext).Assembly);
        }
    }
}
