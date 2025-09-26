using Microsoft.EntityFrameworkCore;
using Saas.Application.Abstractions;
using Saas.Application.Contracts.Plans;
using Saas.Domain.Subscriptions;
using Saas.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Infrastructure.Services
{
    public sealed class PlanService : IPlanService
    {
        private readonly SaasDbContext _db;

        public PlanService(SaasDbContext db) => _db = db;

        public async Task<PlanDto> CreateAsync(CreatePlanRequest req, CancellationToken ct = default)
        {
            var entity = new Plan(req.Name, req.Price, req.DurationDays);
            _db.Plans.Add(entity);
            await _db.SaveChangesAsync(ct);

            return new PlanDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price,
                DurationDays = entity.DurationDays
            };
        }

        public async Task<IReadOnlyList<PlanDto>> ListAsync(CancellationToken ct = default)
        {
            return await _db.Plans
                .AsNoTracking()
                .Select(p => new PlanDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    DurationDays = p.DurationDays
                })
                .ToListAsync(ct);
        }
    }

}
