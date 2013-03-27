using System;
using System.Collections.Generic;
using System.Linq;
using ApiMeta.Common.Attributes;
using ApiMeta.Common.Data;
using ApiMeta.Todos.Models;

namespace ApiMeta.Todos.Controllers
{
    [Meta]
    public class TodosController : ApiMeta.Common.Controllers.AuthorAccessibleController<Todo,Int64>
    {
        public TodosController(IRepository<Todo, Int64> repo) : base(repo) { }

        public IEnumerable<Todo> GetByList(Int64 list)
        {
            return this.repo.Objects.Where(t => t.ListId == list);
        }
    }
}
