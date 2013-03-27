using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiMeta.Common.Auth;

namespace ApiMeta.Common.Filters
{
    interface IAuthenticationRequired
    {
        AuthenticationType Type { get; }
    }
}
