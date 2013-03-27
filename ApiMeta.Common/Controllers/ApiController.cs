using System.Linq;
using System.Web.Http;
using ApiMeta.Common.Data;
using ApiMeta.Common.Exceptions;
using ApiMeta.Common.Filters;
using ApiMeta.Common.Models;

namespace ApiMeta.Common.Controllers
{
    public abstract class ApiController<TResource, TIdentifier> : ApiController
        where TResource : class, IEntity<TIdentifier>
    {
        protected readonly IRepository<TResource, TIdentifier> repo;

        public ApiController(IRepository<TResource, TIdentifier> repo)
        {
            this.repo = repo;
        }

        public virtual TResource Get(TIdentifier id)
        {
            var entity = this.repo.Objects.FirstOrDefault(e => e.Id.Equals(id));

            if (entity == null)
                throw new ResourceNotFoundException<TResource, TIdentifier>(id);

            return entity;
        }

        public virtual IQueryable<TResource> Get()
        {
            return this.repo.Objects;
        }

        public virtual void Delete(TIdentifier id)
        {
            var entity = this.Get(id);

            this.repo.Delete(entity);
        }

        public virtual void Post(TResource entity)
        {
            this.repo.Save(entity);
        }

        public virtual void Put(TIdentifier id, TResource entity)
        {
            entity.Id = id;

            this.repo.Save(entity);
        }
    }
}
