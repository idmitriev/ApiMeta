using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ApiMeta.Common.Auth;

namespace ApiMeta.Common.Models
{
    [DataContract(Name = "apiRequest")]
    public class ApiRequest : DocumentedEntity
    {
        [DataMember(Name = "uri")]
        public String RelativePath
        { 
            get { return this.Uris.FirstOrDefault(); } 
        }

        [DataMember(Name = "uris")]
        public String[] Uris { get; set; }

        [DataMember(Name = "method")]
        public string Method { get;  set; }

        [DataMember(Name = "name")]
        public String Name { get; set; }

        [DataMember(Name = "parameters")]
        public List<ApiRequestParameter> Parameters { get; set; }

        [DataMember(Name = "responseType")]
        public String ResponseType { get; set; }

        [DataMember(Name = "authenticationType")]
        public AuthenticationType AuthenticationType { get; set; }
    }
}
