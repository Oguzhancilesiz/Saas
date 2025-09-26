using Saas.Application.Contracts.Plans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Application.Abstractions
{
    public interface IPlanService
    {
        Task<PlanDto> CreateAsync(CreatePlanRequest req, CancellationToken ct = default);
        Task<IReadOnlyList<PlanDto>> ListAsync(CancellationToken ct = default);
    }

}
