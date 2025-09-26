using Saas.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Domain.Pricing
{
    public sealed class AppPlan : BaseEntity
    {
        public Guid AppId { get; private set; }
        public Guid PlanId { get; private set; }
        public bool IsEnabled { get; private set; } = true;
        public int? TrialDays { get; private set; }   // app'e özel deneme süresi (örn: SPIN 3 gün, DIET 7 gün)

        private AppPlan() { }
        public AppPlan(Guid appId, Guid planId, bool enabled = true, int? trialDays = null)
        { AppId = appId; PlanId = planId; IsEnabled = enabled; TrialDays = trialDays; }
    }

}
