using System;
using ApiMeta.Common.Data;
using ApiMeta.Common.Models;

namespace ApiMeta.Common.Controllers
{
    public class DocumentationController : ApiController<DocumentationArticle, String>
    {
        public DocumentationController(IRepository<DocumentationArticle, String> repo) : base(repo) {}
    }
}
