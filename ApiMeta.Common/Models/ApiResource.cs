using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ApiMeta.Common.Models
{
    [DataContract(Name = "apiResource")]
    public class ApiResource : DocumentedEntity
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "requests")]
        public List<ApiRequest> Requests { get; set; }
    }
}
