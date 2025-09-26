using Saas.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Domain.Features
{
    /// Scope'lu limit kaydı: en özel kural kazanır (User > App+Plan > App|Plan > Global)
    public sealed class FeatureLimit : BaseEntity
    {
        public Guid FeatureId { get; private set; }
        public Guid? AppId { get; private set; }
        public Guid? PlanId { get; private set; }
        public Guid? UserId { get; private set; }
        public UsagePeriod Period { get; private set; }
        public int LimitPerPeriod { get; private set; }
        public int Priority { get; private set; } = 0;
        public DateTime? EffectiveFromUtc { get; private set; }
        public DateTime? EffectiveToUtc { get; private set; }

        private FeatureLimit() { }
        public FeatureLimit(Guid featureId, int limit, UsagePeriod period,
            Guid? appId = null, Guid? planId = null, Guid? userId = null,
            int priority = 0, DateTime? from = null, DateTime? to = null)
        {
            FeatureId = featureId; LimitPerPeriod = limit; Period = period;
            AppId = appId; PlanId = planId; UserId = userId;
            Priority = priority; EffectiveFromUtc = from; EffectiveToUtc = to;
        }
    }
}
