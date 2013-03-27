using System;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web.Http;
using ApiMeta.Common.Auth;
using ApiMeta.Common.Extensions;
using ApiMeta.Common.Membership;

namespace ApiMeta.Common.Filters
{
    public class BasicAuthenticationRequiredAttribute : AuthorizeAttribute, IAuthenticationRequired
    {
        public IMembershipValidator MembershipValidator { get; set; }

        public BasicAuthenticationRequiredAttribute(IMembershipValidator membershipValidator)
        {
            this.MembershipValidator = membershipValidator;
        }

        public BasicAuthenticationRequiredAttribute() { }

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (actionContext == null)
                throw new ArgumentNullException("actionContext");

            var header = actionContext.Request.Headers.FirstOrDefault(h => h.Key == "Authorization");

            if (header.Value == null || !header.Value.Any() || String.IsNullOrEmpty(header.Value.First()))
            {
                base.OnAuthorization(actionContext);
                return;
            }

            var auth = header.Value.First();

            var value = Encoding.ASCII.GetString(Convert.FromBase64String(auth.Replace("Basic ", "")));
            var username = value.Substring(0, value.IndexOf(':'));
            var password = value.Substring(value.IndexOf(':') + 1);

            if (this.MembershipValidator.ValidateUser(username, password))
                actionContext.Request.SetUserPrincipal(new GenericPrincipal(new GenericIdentity(username), null));
            else
                HandleUnauthorizedRequest(actionContext);
        }

        public Auth.AuthenticationType Type
        {
            get { return AuthenticationType.BasicAuth; }
        }
    }
}
