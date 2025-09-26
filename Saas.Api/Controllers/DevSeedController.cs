using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Saas.Application.Abstractions;
using Saas.Application.Contracts.Plans;
using Saas.Domain.Apps;
using Saas.Domain.Features;
using Saas.Domain.Pricing;
using Saas.Infrastructure.Persistence;

namespace Saas.Api.Controllers
{
    [ApiController]
    [Route("api/dev-seed")]
    public sealed class DevSeedController : ControllerBase
    {
        private readonly SaasDbContext _db;
        private readonly IPlanService _plans;

        public DevSeedController(SaasDbContext db, IPlanService plans)
        { _db = db; _plans = plans; }

        [HttpPost]
        public async Task<IActionResult> Seed(CancellationToken ct)
        {
            // 1) Apps
            if (!await _db.Apps.AnyAsync(ct))
            {
                _db.Apps.AddRange(
                    new App("Hesap Çarkı", "SPIN"),
                    new App("Diyet", "DIET")
                );
                await _db.SaveChangesAsync(ct);
            }

            // 2) Plans (eski Plan.Price dursun ama referans fiyatı artık PlanPrice/AppPlanPrice'ta)
            if (!await _db.Plans.AnyAsync(ct))
            {
                await _plans.CreateAsync(new CreatePlanRequest { Name = "Basic", Price = 0, DurationDays = 7 }, ct);
                await _plans.CreateAsync(new CreatePlanRequest { Name = "Pro", Price = 99, DurationDays = 30 }, ct);
            }
            await _db.SaveChangesAsync(ct);

            var spin = await _db.Apps.AsNoTracking().FirstAsync(a => a.Code == "SPIN", ct);
            var diet = await _db.Apps.AsNoTracking().FirstAsync(a => a.Code == "DIET", ct);
            var basic = await _db.Plans.AsNoTracking().FirstAsync(p => p.Name == "Basic", ct);
            var pro = await _db.Plans.AsNoTracking().FirstAsync(p => p.Name == "Pro", ct);

            // 3) Features (her seferinde DB’den al, boş GUID tuzağına düşme)
            var fSpin = await _db.Set<Feature>().FirstOrDefaultAsync(f => f.Code == "SPIN_START", ct);
            var fDiet = await _db.Set<Feature>().FirstOrDefaultAsync(f => f.Code == "DIET_PROGRAM_CREATE", ct);
            if (fSpin is null) { fSpin = new Feature("SPIN_START", "Çark başlatma"); _db.Add(fSpin); }
            if (fDiet is null) { fDiet = new Feature("DIET_PROGRAM_CREATE", "Diyet programı oluşturma"); _db.Add(fDiet); }
            await _db.SaveChangesAsync(ct);

            // AppFeature bağları (özelliği app'te aç/kapat)
            if (!await _db.Set<AppFeature>().AnyAsync(ct))
            {
                _db.AddRange(
                    new AppFeature(spin.Id, fSpin.Id, true),
                    new AppFeature(diet.Id, fDiet.Id, true)
                );
                await _db.SaveChangesAsync(ct);
            }

            // 4) AppPlan + trial günleri
            if (!await _db.Set<AppPlan>().AnyAsync(ct))
            {
                _db.AddRange(
                    new AppPlan(spin.Id, basic.Id, true, trialDays: 3),   // SPIN: Basic 3 gün
                    new AppPlan(spin.Id, pro.Id, true, trialDays: 0),
                    new AppPlan(diet.Id, basic.Id, true, trialDays: 7),   // DIET: Basic 7 gün
                    new AppPlan(diet.Id, pro.Id, true, trialDays: 0)
                );
                await _db.SaveChangesAsync(ct);
            }

            // 5) Fiyatlar: global ve app’e özel
            if (!await _db.Set<PlanPrice>().AnyAsync(ct))
            {
                _db.AddRange(
                    new PlanPrice(basic.Id, BillingPeriod.Monthly, 0m),
                    new PlanPrice(pro.Id, BillingPeriod.Monthly, 199m)
                );
                await _db.SaveChangesAsync(ct);
            }

            if (!await _db.Set<AppPlanPrice>().AnyAsync(ct))
            {
                _db.AddRange(
                    new AppPlanPrice(diet.Id, pro.Id, BillingPeriod.Monthly, 249m), // DIET Pro 249
                    new AppPlanPrice(spin.Id, pro.Id, BillingPeriod.Monthly, 0m, model: PricingModel.MeteredPerUnit) // SPIN Pro metered
                );
                await _db.SaveChangesAsync(ct);
            }

            // Metered kademeler (SPIN Pro)
            var spinProAppPrice = await _db.Set<AppPlanPrice>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.AppId == spin.Id && x.PlanId == pro.Id, ct);

            if (spinProAppPrice is not null &&
                !await _db.Set<PriceTier>().AnyAsync(t => t.AppPlanPriceId == spinProAppPrice.Id, ct))
            {
                _db.AddRange(
                    new PriceTier(0, 1000, 0.10m, appPlanPriceId: spinProAppPrice.Id),
                    new PriceTier(1001, int.MaxValue, 0.08m, appPlanPriceId: spinProAppPrice.Id)
                );
                await _db.SaveChangesAsync(ct);
            }

            // 6) Scope'lu limitler (YENİ MODEL) - PlanFeatureLimit'i artık seed etmiyoruz
            if (!await _db.Set<FeatureLimit>().AnyAsync(ct))
            {
                _db.AddRange(
                    // SPIN: Basic günlük 300, Pro günlük 1000
                    new FeatureLimit(fSpin.Id, 300, UsagePeriod.Daily, appId: spin.Id, planId: basic.Id),
                    new FeatureLimit(fSpin.Id, 1000, UsagePeriod.Daily, appId: spin.Id, planId: pro.Id),
                    // DIET: Basic aylık 50, Pro aylık 500
                    new FeatureLimit(fDiet.Id, 50, UsagePeriod.Monthly, appId: diet.Id, planId: basic.Id),
                    new FeatureLimit(fDiet.Id, 500, UsagePeriod.Monthly, appId: diet.Id, planId: pro.Id)
                );
                await _db.SaveChangesAsync(ct);
            }

            return Ok(new { status = "ok" });
        }
    }
}
