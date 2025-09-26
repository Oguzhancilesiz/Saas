using Microsoft.EntityFrameworkCore;
using Saas.Application.Abstractions;
using Saas.Domain.Usage;
using Saas.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Infrastructure.Services
{
    public sealed class UsageService : IUsageService
    {
        private readonly SaasDbContext _db;

        public UsageService(SaasDbContext db) => _db = db;

        public async Task LogAsync(Guid userId, Guid appId, string featureCode, int quantity = 1, CancellationToken ct = default)
        {
            var now = DateTime.UtcNow;
            _db.Add(new UsageLog(userId, appId, featureCode, quantity, now));
            await _db.SaveChangesAsync(ct);
        }

        public Task<int> CountAsync(Guid userId, Guid appId, string featureCode, DateTime fromUtc, DateTime toUtc, CancellationToken ct = default)
        {
            return _db.Set<UsageLog>()
                .AsNoTracking()
                .Where(x => x.UserId == userId && x.AppId == appId && x.FeatureCode == featureCode
                            && x.OccurredAtUtc >= fromUtc && x.OccurredAtUtc <= toUtc)
                .SumAsync(x => x.Quantity, ct);
        }
    }

}
