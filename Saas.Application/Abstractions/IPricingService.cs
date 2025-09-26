using Saas.Domain.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Application.Abstractions
{
    public sealed record EffectivePrice(string Currency, decimal Amount, BillingPeriod Period, PricingModel Model,
                                      IReadOnlyList<(int from, int to, decimal unitPrice)>? Tiers,
                                      int? TrialDays);

    public interface IPricingService
    {
        Task<EffectivePrice?> GetEffectivePriceAsync(Guid appId, Guid planId, DateTime utcNow, CancellationToken ct = default);
    }
}
