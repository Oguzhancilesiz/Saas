using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Infrastructure.Auth
{
    public sealed class AppUser : IdentityUser<Guid>
    {
        // İlerde TenantId, DisplayName vs. ekleyebilirsin
    }

}
