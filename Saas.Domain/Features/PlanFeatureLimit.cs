using Saas.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Domain.Features
{
    public sealed class PlanFeatureLimit : BaseEntity
    {
        public Guid PlanId { get; private set; }
        public Guid FeatureId { get; private set; }
        public int LimitPerPeriod { get; private set; }        // örn: 100
        public UsagePeriod Period { get; private set; }        // Daily / Monthly

        private PlanFeatureLimit() { }
        public PlanFeatureLimit(Guid planId, Guid featureId, int limit, UsagePeriod period)
        {
            PlanId = planId; FeatureId = featureId; LimitPerPeriod = limit; Period = period;
        }
    }

}
