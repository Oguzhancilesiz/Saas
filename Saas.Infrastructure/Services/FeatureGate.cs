using Microsoft.EntityFrameworkCore;
using Saas.Application.Abstractions;
using Saas.Domain.Features;
using Saas.Infrastructure.Persistence;

namespace Saas.Infrastructure.Services;

public sealed class FeatureGate : IFeatureGate
{
    private readonly SaasDbContext _db;
    private readonly IUsageService _usage;

    public FeatureGate(SaasDbContext db, IUsageService usage) { _db = db; _usage = usage; }

    public async Task<bool> IsAllowedAsync(Guid userId, Guid appId, string featureCode, CancellationToken ct = default)
    {
        var feature = await _db.Set<Feature>().AsNoTracking().FirstOrDefaultAsync(x => x.Code == featureCode, ct);
        if (feature is null) return false;

        var now = DateTime.UtcNow;

        // Aktif abonelikten planId al
        var sub = await _db.Subscriptions.AsNoTracking()
            .Where(s => s.UserId == userId && s.AppId == appId && s.StartUtc <= now && s.EndUtc >= now)
            .OrderByDescending(s => s.EndUtc)
            .FirstOrDefaultAsync(ct);
        var planId = sub?.PlanId;

        // Aynı feature için uygun tarihli tüm limitleri çek
        var limits = await _db.Set<FeatureLimit>().AsNoTracking()
            .Where(x => x.FeatureId == feature.Id
                && (x.AppId == null || x.AppId == appId)
                && (x.PlanId == null || (planId != null && x.PlanId == planId))
                && (x.UserId == null || x.UserId == userId)
                && (x.EffectiveFromUtc == null || x.EffectiveFromUtc <= now)
                && (x.EffectiveToUtc == null || x.EffectiveToUtc >= now))
            .ToListAsync(ct);

        if (limits.Count == 0) return true; // kural yoksa serbest

        // En spesifik kuralı seç
        var chosen = limits
            .OrderByDescending(x => x.UserId.HasValue)
            .ThenByDescending(x => x.AppId.HasValue && x.PlanId.HasValue)
            .ThenByDescending(x => x.AppId.HasValue || x.PlanId.HasValue)
            .ThenByDescending(x => x.Priority)
            .ThenByDescending(x => x.EffectiveFromUtc)
            .First();

        var (from, to) = GetWindow(chosen.Period, now);
        var used = await _usage.CountAsync(userId, appId, featureCode, from, to, ct);
        return used + 1 <= chosen.LimitPerPeriod;
    }

    public async Task<bool> CheckAndLogAsync(Guid userId, Guid appId, string featureCode, int quantity = 1, CancellationToken ct = default)
    {
        if (!await IsAllowedAsync(userId, appId, featureCode, ct)) return false;
        await _usage.LogAsync(userId, appId, featureCode, quantity, ct);
        return true;
    }

    private static (DateTime from, DateTime to) GetWindow(UsagePeriod p, DateTime now) =>
        p switch
        {
            UsagePeriod.Daily => (now.Date, now.Date.AddDays(1).AddTicks(-1)),
            UsagePeriod.Monthly => (new DateTime(now.Year, now.Month, 1), new DateTime(now.Year, now.Month, 1).AddMonths(1).AddTicks(-1)),
            UsagePeriod.Lifetime => (DateTime.UnixEpoch, DateTime.MaxValue.AddDays(-1)),
            _ => (now.Date, now.Date.AddDays(1).AddTicks(-1))
        };
}
