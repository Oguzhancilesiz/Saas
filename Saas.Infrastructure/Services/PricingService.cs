using Microsoft.EntityFrameworkCore;
using Saas.Application.Abstractions;
using Saas.Domain.Pricing;
using Saas.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Infrastructure.Services
{
    public sealed class PricingService : IPricingService
    {
        private readonly SaasDbContext _db;
        public PricingService(SaasDbContext db) { _db = db; }

        public async Task<EffectivePrice?> GetEffectivePriceAsync(Guid appId, Guid planId, DateTime now, CancellationToken ct = default)
        {
            // App'e özel fiyat varsa onu, yoksa global plan fiyatını al
            var appPrice = await _db.Set<AppPlanPrice>().AsNoTracking()
                .Where(x => x.AppId == appId && x.PlanId == planId
                    && (x.EffectiveFromUtc == null || x.EffectiveFromUtc <= now)
                    && (x.EffectiveToUtc == null || x.EffectiveToUtc >= now))
                .OrderByDescending(x => x.EffectiveFromUtc)
                .FirstOrDefaultAsync(ct);

            if (appPrice is not null)
            {
                var tiers = await _db.Set<PriceTier>().AsNoTracking()
                    .Where(t => t.AppPlanPriceId == appPrice.Id).ToListAsync(ct);
                var appPlan = await _db.Set<AppPlan>().AsNoTracking()
                    .FirstOrDefaultAsync(x => x.AppId == appId && x.PlanId == planId, ct);

                return new EffectivePrice(appPrice.Currency, appPrice.Amount, appPrice.BillingPeriod, appPrice.Model,
                    tiers.Select(t => (t.FromQuantity, t.ToQuantity, t.UnitPrice)).ToList(), appPlan?.TrialDays);
            }

            var planPrice = await _db.Set<PlanPrice>().AsNoTracking()
                .Where(x => x.PlanId == planId
                    && (x.EffectiveFromUtc == null || x.EffectiveFromUtc <= now)
                    && (x.EffectiveToUtc == null || x.EffectiveToUtc >= now))
                .OrderByDescending(x => x.EffectiveFromUtc)
                .FirstOrDefaultAsync(ct);

            if (planPrice is null) return null;

            var tiers2 = await _db.Set<PriceTier>().AsNoTracking()
                .Where(t => t.PlanPriceId == planPrice.Id).ToListAsync(ct);
            var appPlan2 = await _db.Set<AppPlan>().AsNoTracking()
                .FirstOrDefaultAsync(x => x.AppId == appId && x.PlanId == planId, ct);

            return new EffectivePrice(planPrice.Currency, planPrice.Amount, planPrice.BillingPeriod, planPrice.Model,
                tiers2.Select(t => (t.FromQuantity, t.ToQuantity, t.UnitPrice)).ToList(), appPlan2?.TrialDays);
        }
    }

}
