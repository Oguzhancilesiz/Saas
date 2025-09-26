using Saas.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Domain.Apps
{
    public class App : BaseEntity
    {
        public string Name { get; private set; } = default!;
        public string Code { get; private set; } = default!; // "SPIN", "DIET" gibi

        private App() { }
        public App(string name, string code)
        {
            Name = name; Code = code;
        }
    }

}
