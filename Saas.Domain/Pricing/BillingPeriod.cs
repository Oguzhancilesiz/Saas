using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Domain.Pricing
{
    public enum BillingPeriod { Monthly = 1, Yearly = 2, OneTime = 3 }
    public enum PricingModel { Flat = 1, Tiered = 2, MeteredPerUnit = 3 }
}
