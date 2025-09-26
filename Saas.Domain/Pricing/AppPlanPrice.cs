using Saas.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Domain.Pricing
{
    public sealed class AppPlanPrice : BaseEntity
    {
        public Guid AppId { get; private set; }
        public Guid PlanId { get; private set; }
        public string Currency { get; private set; } = "TRY";
        public BillingPeriod BillingPeriod { get; private set; }
        public PricingModel Model { get; private set; } = PricingModel.Flat;
        public decimal Amount { get; private set; }
        public DateTime? EffectiveFromUtc { get; private set; }
        public DateTime? EffectiveToUtc { get; private set; }
        private AppPlanPrice() { }
        public AppPlanPrice(Guid appId, Guid planId, BillingPeriod bp, decimal amount, string currency = "TRY",
                            PricingModel model = PricingModel.Flat, DateTime? from = null, DateTime? to = null)
        { AppId = appId; PlanId = planId; BillingPeriod = bp; Amount = amount; Currency = currency; Model = model; EffectiveFromUtc = from; EffectiveToUtc = to; }
    }

}
