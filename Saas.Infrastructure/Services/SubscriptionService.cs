using Microsoft.EntityFrameworkCore;
using Saas.Application.Abstractions;
using Saas.Application.Contracts.Subscriptions;
using Saas.Domain.Subscriptions;
using Saas.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Infrastructure.Services
{
    public sealed class SubscriptionService : ISubscriptionService
    {
        private readonly SaasDbContext _db;
        private readonly IDateTimeProvider _clock;

        public SubscriptionService(SaasDbContext db, IDateTimeProvider clock)
        {
            _db = db;
            _clock = clock;
        }

        public async Task<SubscriptionDto> SubscribeAsync(SubscribeRequest req, CancellationToken ct = default)
        {
            // Plan’ı çek
            var plan = await _db.Plans.FirstOrDefaultAsync(x => x.Id == req.PlanId, ct)
                       ?? throw new InvalidOperationException("Plan not found");

            var start = _clock.UtcNow;
            var end = start.AddDays(plan.DurationDays);

            var sub = new Subscription(req.UserId, req.AppId, req.PlanId, start, end);
            _db.Subscriptions.Add(sub);
            await _db.SaveChangesAsync(ct);

            return new SubscriptionDto
            {
                Id = sub.Id,
                UserId = sub.UserId,
                AppId = sub.AppId,
                PlanId = sub.PlanId,
                StartUtc = sub.StartUtc,
                EndUtc = sub.EndUtc,
                IsActive = sub.IsActive
            };
        }

        public async Task<IReadOnlyList<SubscriptionDto>> GetUserSubscriptionsAsync(Guid userId, Guid appId, CancellationToken ct = default)
        {
            return await _db.Subscriptions
                .AsNoTracking()
                .Where(s => s.UserId == userId && s.AppId == appId)
                .OrderByDescending(s => s.StartUtc)
                .Select(s => new SubscriptionDto
                {
                    Id = s.Id,
                    UserId = s.UserId,
                    AppId = s.AppId,
                    PlanId = s.PlanId,
                    StartUtc = s.StartUtc,
                    EndUtc = s.EndUtc,
                    IsActive = s.IsActive
                })
                .ToListAsync(ct);
        }
    }

}
