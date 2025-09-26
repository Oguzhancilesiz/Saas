using Saas.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Domain.Pricing
{
    public sealed class PriceTier : BaseEntity
    {
        public Guid? PlanPriceId { get; private set; }
        public Guid? AppPlanPriceId { get; private set; }
        public int FromQuantity { get; private set; }  // dahil
        public int ToQuantity { get; private set; }    // dahil
        public decimal UnitPrice { get; private set; }

        private PriceTier() { }
        public PriceTier(int from, int to, decimal unitPrice, Guid? planPriceId = null, Guid? appPlanPriceId = null)
        { FromQuantity = from; ToQuantity = to; UnitPrice = unitPrice; PlanPriceId = planPriceId; AppPlanPriceId = appPlanPriceId; }
    }

}
