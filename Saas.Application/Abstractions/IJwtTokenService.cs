using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Application.Abstractions
{
    public interface IJwtTokenService
    {
        string CreateToken(Guid userId, string email, IEnumerable<string> roles);
    }

}
