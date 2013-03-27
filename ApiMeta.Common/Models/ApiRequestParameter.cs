using System;
using System.Runtime.Serialization;

namespace ApiMeta.Common.Models
{
    [DataContract(Name = "requestPatameter")]
    public class ApiRequestParameter : DocumentedEntity
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "source")]
        public string Source { get; set; }

        [DataMember(Name = "type")]
        public String Type { get; set; }
    }
}

