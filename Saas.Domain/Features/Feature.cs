using Saas.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Domain.Features
{
    public sealed class Feature : BaseEntity
    {
        public string Code { get; private set; } = default!;   // "SPIN_START"
        public string Name { get; private set; } = default!;
        private Feature() { }
        public Feature(string code, string name) { Code = code; Name = name; }
    }
}
