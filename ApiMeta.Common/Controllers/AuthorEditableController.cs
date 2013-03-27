using ApiMeta.Common.Data;
using ApiMeta.Common.Exceptions;
using ApiMeta.Common.Extensions;
using ApiMeta.Common.Filters;
using ApiMeta.Common.Models;

namespace ApiMeta.Common.Controllers
{
    public abstract class AuthorEditableController<TResource, TIdentifier> : ApiController<TResource, TIdentifier>
        where TResource : class, IAuthoredEntity<TIdentifier>
    {
        protected AuthorEditableController(IRepository<TResource, TIdentifier> repo) : base(repo) { }

        [BasicAuthenticationRequired]
        public override void Delete(TIdentifier id)
        {
            var entity = this.Get(id);

            if (entity.Author != this.Request.GetUserPrincipal().Identity.Name)
                throw new AccessForbiddenException();

            base.Delete(id);
        }

        [BasicAuthenticationRequired]
        public override void Post(TResource entity)
        {
            entity.Author = this.Request.GetUserPrincipal().Identity.Name;

            base.Post(entity);
        }

        [BasicAuthenticationRequired]
        public override void Put(TIdentifier id, TResource entity)
        {
            var existingEntity = this.Get(id);

            if ( entity.Author != this.Request.GetUserPrincipal().Identity.Name )
                throw new AccessForbiddenException();

            base.Put(id,entity);
        }
    }
}
