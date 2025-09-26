using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Saas.Application.Abstractions;
using Saas.Infrastructure.Persistence;

namespace Saas.Api.Controllers;

[ApiController]
[Route("api/apps/{appCode}/plans")]
public sealed class AppPlansController : ControllerBase
{
    private readonly SaasDbContext _db;
    private readonly IPricingService _pricing;

    public AppPlansController(SaasDbContext db, IPricingService pricing)
    { _db = db; _pricing = pricing; }

    [HttpGet]
    public async Task<IActionResult> List([FromRoute] string appCode, CancellationToken ct)
    {
        var app = await _db.Apps.AsNoTracking().FirstOrDefaultAsync(a => a.Code == appCode, ct);
        if (app is null) return NotFound("App not found");

        // Bu app'te aktif planlar
        var planIds = await _db.Set<Saas.Domain.Pricing.AppPlan>().AsNoTracking()
            .Where(x => x.AppId == app.Id && x.IsEnabled)
            .Select(x => x.PlanId).ToListAsync(ct);

        var plans = await _db.Plans.AsNoTracking().Where(p => planIds.Contains(p.Id)).ToListAsync(ct);

        var result = new List<object>();
        foreach (var p in plans)
        {
            var eff = await _pricing.GetEffectivePriceAsync(app.Id, p.Id, DateTime.UtcNow, ct);
            result.Add(new
            {
                planId = p.Id,
                p.Name,
                price = eff is null ? null : new { eff.Currency, eff.Amount, period = eff.Period.ToString(), model = eff.Model.ToString(), eff.TrialDays, tiers = eff.Tiers }
            });
        }
        return Ok(result);
    }
}
