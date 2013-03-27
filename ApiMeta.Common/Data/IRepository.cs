using System.Linq;
using ApiMeta.Common.Models;

namespace ApiMeta.Common.Data
{
    public interface IRepository<TEntity, TIdentifier>
        where TEntity : class, IEntity<TIdentifier>
    {
        void Save(TEntity entity);
        IQueryable<TEntity> Objects { get; }
        void Delete(TEntity entity);
    }
}
