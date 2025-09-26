using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Application.Abstractions
{
    public interface IUsageService
    {
        Task LogAsync(Guid userId, Guid appId, string featureCode, int quantity = 1, CancellationToken ct = default);
        Task<int> CountAsync(Guid userId, Guid appId, string featureCode, DateTime fromUtc, DateTime toUtc, CancellationToken ct = default);
    }

}
