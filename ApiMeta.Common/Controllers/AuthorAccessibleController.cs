using System.Linq;
using ApiMeta.Common.Data;
using ApiMeta.Common.Exceptions;
using ApiMeta.Common.Extensions;
using ApiMeta.Common.Filters;
using ApiMeta.Common.Models;

namespace ApiMeta.Common.Controllers
{
    public abstract class AuthorAccessibleController<TResource, TIdentifier> : AuthorEditableController<TResource, TIdentifier>
        where TResource : class, IAuthoredEntity<TIdentifier>
    {
        protected AuthorAccessibleController(IRepository<TResource, TIdentifier> repo) : base(repo)
        {
        }

        [BasicAuthenticationRequired]
        public override TResource Get(TIdentifier id)
        {
            var entity = base.Get(id);

            if (entity.Author != this.Request.GetUserPrincipal().Identity.Name)
                throw new AccessForbiddenException();

            return entity;
        }

        [BasicAuthenticationRequired]
        public override IQueryable<TResource> Get()
        {
            return base.Get().Where(e => e.Author == this.Request.GetUserPrincipal().Identity.Name);
        }
    }
}
