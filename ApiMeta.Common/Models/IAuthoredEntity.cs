using System;
using ApiMeta.Common.Data;

namespace ApiMeta.Common.Models
{
    public interface IAuthoredEntity<TIdentifier> : IEntity<TIdentifier>
    {
        String Author { get; set; }
    }
}
