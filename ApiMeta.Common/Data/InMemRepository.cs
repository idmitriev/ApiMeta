using System.Collections.Generic;
using System.Linq;
using ApiMeta.Common.Models;

namespace ApiMeta.Common.Data
{
    public class InMemRepository<TEntity, TIdentifier> : IRepository<TEntity, TIdentifier>
        where TEntity : class, IEntity<TIdentifier>
    {
        protected readonly IList<TEntity> objects = new List<TEntity>();

        public void Save(TEntity entity)
        {
            if (!this.objects.Contains(entity) )
                this.objects.Add(entity);
        }

        public IQueryable<TEntity> Objects
        {
            get { return this.objects.AsQueryable(); } 
        }

        public void Delete(TEntity entity)
        {
            if (this.objects.Contains(entity))
                this.objects.Remove(entity);
        }
    }
}
