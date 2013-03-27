using System;
using System.Runtime.Serialization;

namespace ApiMeta.Common.Models
{
    [DataContract(Name = "documentedEntity")]
    public abstract class DocumentedEntity
    {
        [DataMember(Name = "docId")]
        public String DocumentationArticleId { get; set; }
    }
}
