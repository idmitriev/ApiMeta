using System;
using ApiMeta.Common.Attributes;
using ApiMeta.Common.Data;
using ApiMeta.Todos.Models;

namespace ApiMeta.Todos.Controllers
{
    [Meta]
    public class TodoListsController : ApiMeta.Common.Controllers.AuthorAccessibleController<TodoList, Int64>
    {
        public TodoListsController(IRepository<TodoList, long> repo) : base(repo) {  }
    }
}
