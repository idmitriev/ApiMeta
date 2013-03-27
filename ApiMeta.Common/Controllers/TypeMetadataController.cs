using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Web.Http;
using ApiMeta.Common.Attributes;
using ApiMeta.Common.Exceptions;
using ApiMeta.Common.Filters;
using ApiMeta.Common.Models;

namespace ApiMeta.Common.Controllers
{
    public class TypeMetadataController : ApiController
    {
        private readonly Assembly typeAssembly;
        private readonly IList<String> namespaces;

        public TypeMetadataController(Assembly typeAssembly, IList<String> namespaces = null)
        {
            this.typeAssembly = typeAssembly;
            this.namespaces = namespaces;
        }

        [OutputCache(60,60,false)]
        public IEnumerable<ApiType> Get()
        {
            return this.typeAssembly
                .GetTypes()
                .Where(t =>
                    (this.namespaces == null ||  this.namespaces.Contains(t.Namespace)) &&
                    Attribute.IsDefined(t, typeof(MetaAttribute))
                )
                .Select(GetApiType);
        }

        [OutputCache(60, 60, false)]
        public ApiType Get(String name)
        {
            var type = this.Get().FirstOrDefault(t => t.Name == name);
            if (type == null)
                throw new ResourceNotFoundException<ApiType, String>(name);

            return type;
        }

        ApiType GetApiType(Type type)
        {
            var dataContractAttribute = type.GetCustomAttribute<DataContractAttribute>();

            return new ApiType
            {
                Name = dataContractAttribute != null ? dataContractAttribute.Name : type.Name,
                DocumentationArticleId = dataContractAttribute != null ? dataContractAttribute.Name : type.Name,
                Properties = type.GetMembers()
                            .Where(p => p.IsDefined(typeof(DataMemberAttribute), false))
                            .Select(p =>
                            {
                                var dataMemberAttribute = p.GetCustomAttributes(typeof (DataMemberAttribute), false).First() as DataMemberAttribute;
                                return new ApiTypeProperty
                                {
                                    Name = dataMemberAttribute != null ? dataMemberAttribute.Name : p.Name,
                                    Type = ApiType.GetTypeName(GetMemberUnderlyingType(p)),
                                    DocumentationArticleId = String.Format("{0}.{1}", dataContractAttribute != null ? dataContractAttribute.Name : type.Name, dataMemberAttribute != null ? dataMemberAttribute.Name : p.Name)
                                };
                            }
                ).ToList()
            };
        }

        static Type GetMemberUnderlyingType(MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                default:
                    throw new ArgumentException("MemberInfo must be if type FieldInfo or PropertyInfo", "member");
            }
        }
    }
}
