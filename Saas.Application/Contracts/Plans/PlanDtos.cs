using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Application.Contracts.Plans
{
    public sealed class PlanDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
        public decimal Price { get; init; }
        public int DurationDays { get; init; }
    }
    public sealed class CreatePlanRequest
    {
        public string Name { get; init; } = default!;
        public decimal Price { get; init; }
        public int DurationDays { get; init; }
    }

}
