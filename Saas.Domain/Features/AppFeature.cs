using Saas.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Domain.Features
{
    public sealed class AppFeature : BaseEntity
    {
        public Guid AppId { get; private set; }
        public Guid FeatureId { get; private set; }
        public bool IsEnabled { get; private set; } = true;

        private AppFeature() { }
        public AppFeature(Guid appId, Guid featureId, bool enabled = true)
        {
            AppId = appId; FeatureId = featureId; IsEnabled = enabled;
        }
    }

}
