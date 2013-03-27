using System;
using System.Net.Http;
using System.Security.Principal;

namespace ApiMeta.Common.Extensions
{
    public static class RequestExtensions
    {
        const String key = "MS_UserPrincipal";

        public static IPrincipal GetUserPrincipal(this HttpRequestMessage request)
        {
            return (IPrincipal)request.Properties[key];
        }

        public static void SetUserPrincipal(this HttpRequestMessage request, IPrincipal principal)
        {
            request.Properties[key] = principal;
        }
    }
}
