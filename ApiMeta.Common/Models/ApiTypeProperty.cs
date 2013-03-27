using System;
using System.Runtime.Serialization;

namespace ApiMeta.Common.Models
{
    [DataContract(Name = "property")]
    public class ApiTypeProperty : DocumentedEntity
    {
        [DataMember(Name = "name")]
        public String Name { get; set; }

        [DataMember(Name = "type")]
        public String Type { get; set; }
    }
}