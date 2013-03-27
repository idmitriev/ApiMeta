using System.Web.Http;

namespace ApiMeta.Common.Exceptions
{
    class ResourceNotFoundException<TResource, TIdentifier> : HttpResponseException
    {
        public ResourceNotFoundException(TIdentifier id) : base(System.Net.HttpStatusCode.NotFound) { }
    }
}
