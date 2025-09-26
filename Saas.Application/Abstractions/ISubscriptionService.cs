using Saas.Application.Contracts.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Application.Abstractions
{
    public interface ISubscriptionService
    {
        Task<SubscriptionDto> SubscribeAsync(SubscribeRequest req, CancellationToken ct = default);
        Task<IReadOnlyList<SubscriptionDto>> GetUserSubscriptionsAsync(Guid userId, Guid appId, CancellationToken ct = default);
    }

}
