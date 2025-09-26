using Saas.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.SharedKernel
{
    public abstract class BaseEntity
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
        public DateTime? UpdatedAtUtc { get; private set; }
        public RecordStatus Status { get; private set; } = RecordStatus.Active;

        public void MarkUpdated() => UpdatedAtUtc = DateTime.UtcNow;
        public void SoftDelete() => Status = RecordStatus.Deleted;
    }
}
