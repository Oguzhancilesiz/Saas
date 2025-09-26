using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Application.Contracts.Subscriptions
{
    public sealed record SubscribeRequest
    {
        public Guid UserId { get; init; }
        public Guid AppId { get; init; }
        public Guid PlanId { get; init; }
    }
    public sealed class SubscriptionDto
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public Guid AppId { get; init; }
        public Guid PlanId { get; init; }
        public DateTime StartUtc { get; init; }
        public DateTime EndUtc { get; init; }
        public bool IsActive { get; init; }
    }

}
