using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using ApiMeta.Common.Attributes;
using ApiMeta.Common.Auth;
using ApiMeta.Common.Filters;
using ApiMeta.Common.Models;

namespace ApiMeta.Common.Controllers
{
    public class ResourceMetadataController : ApiController
    {
        private readonly IApiExplorer apiExplorer;

        public ResourceMetadataController(IApiExplorer apiExplorer)
        {
            this.apiExplorer = apiExplorer;
        }

        [OutputCache(60,60,false)]
        public IEnumerable<ApiResource> Get()
        {
            var controllers = this.apiExplorer
               .ApiDescriptions
               .Where(x => x.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<MetaAttribute>().Any() || x.ActionDescriptor.GetCustomAttributes<MetaAttribute>().Any())
               .GroupBy(x => x.ActionDescriptor.ControllerDescriptor.ControllerName)
               .Select(x => x.First().ActionDescriptor.ControllerDescriptor.ControllerName)
               .ToList();

            return controllers.Select(GetApiResourceMetadata).ToList();
        }

        ApiResource GetApiResourceMetadata(string controller)
        {
            var apis = this.apiExplorer
             .ApiDescriptions
             .Where(x =>
                 x.ActionDescriptor.ControllerDescriptor.ControllerName == controller &&
                 ( x.ActionDescriptor.GetCustomAttributes<MetaAttribute>().Any() || x.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<MetaAttribute>().Any() )
             ).GroupBy(x => x.ActionDescriptor);
            
            return new ApiResource
            {
                Name = controller,
                Requests = apis.Select(g => this.GetApiRequest(g.First(), g.Select(d => d.RelativePath))).ToList(),
                DocumentationArticleId = controller
            };
        }

        ApiRequest GetApiRequest(ApiDescription api, IEnumerable<String> uris)
        {
            return new ApiRequest
            {
                Name = api.ActionDescriptor.ActionName,
                Uris = uris.ToArray(),
                DocumentationArticleId = String.Format("{0}.{1}", api.ActionDescriptor.ControllerDescriptor.ControllerName, api.ActionDescriptor.ActionName),
                Method = api.HttpMethod.Method,
                Parameters = api.ParameterDescriptions.Select( parameter => 
                    new ApiRequestParameter
                    {
                        Name = parameter.Name,
                        DocumentationArticleId = String.Format("{0}.{1}.{2}", api.ActionDescriptor.ControllerDescriptor.ControllerName, api.ActionDescriptor.ActionName, parameter.Name),
                        Source = parameter.Source.ToString().ToLower().Replace("from",""),
                        Type = ApiType.GetTypeName(parameter.ParameterDescriptor.ParameterType)
                    }).ToList(),
                ResponseType = ApiType.GetTypeName(api.ActionDescriptor.ReturnType),
                AuthenticationType = !api.ActionDescriptor.GetCustomAttributes<IAuthenticationRequired>().Any() ? AuthenticationType.None : api.ActionDescriptor.GetCustomAttributes<IAuthenticationRequired>().First().Type
            };
        }
    }
}
