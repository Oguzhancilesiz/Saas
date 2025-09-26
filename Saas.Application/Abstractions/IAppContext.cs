using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Application.Abstractions
{
    public interface IAppContext
    {
        Guid? AppId { get; }
        string? AppCode { get; }
    }

}
