using Saas.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Domain.Usage
{
    public sealed class UsageLog : BaseEntity
    {
        public Guid UserId { get; private set; }
        public Guid AppId { get; private set; }
        public string FeatureCode { get; private set; } = default!;  // "SPIN_START", "DIET_PROGRAM_CREATE"
        public int Quantity { get; private set; } = 1;
        public DateTime OccurredAtUtc { get; private set; }

        private UsageLog() { }

        public UsageLog(Guid userId, Guid appId, string featureCode, int quantity, DateTime occurredAtUtc)
        {
            UserId = userId;
            AppId = appId;
            FeatureCode = featureCode;
            Quantity = quantity;
            OccurredAtUtc = occurredAtUtc;
        }
    }

}
