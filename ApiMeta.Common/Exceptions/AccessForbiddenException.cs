using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApiMeta.Common.Exceptions
{
    public class AccessForbiddenException : HttpResponseException
    {
        public AccessForbiddenException()
            : base(System.Net.HttpStatusCode.Forbidden)
        {
        }

    }
}