using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Application.Abstractions
{
    public interface IFeatureGate
    {
        Task<bool> IsAllowedAsync(Guid userId, Guid appId, string featureCode, CancellationToken ct = default);
        Task<bool> CheckAndLogAsync(Guid userId, Guid appId, string featureCode, int quantity = 1, CancellationToken ct = default);
    }
}
