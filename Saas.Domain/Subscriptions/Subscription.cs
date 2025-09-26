using Saas.SharedKernel;
using Saas.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Domain.Subscriptions
{
    public class Subscription : BaseEntity
    {
        public Guid UserId { get; private set; }
        public Guid AppId { get; private set; }
        public Guid PlanId { get; private set; }
        public DateTime StartUtc { get; private set; }
        public DateTime EndUtc { get; private set; }
        public bool IsActive => DateTime.UtcNow <= EndUtc && Status == RecordStatus.Active;

        private Subscription() { }
        public Subscription(Guid userId, Guid appId, Guid planId, DateTime startUtc, DateTime endUtc)
        {
            UserId = userId; AppId = appId; PlanId = planId;
            StartUtc = startUtc; EndUtc = endUtc;
        }
    }

}
