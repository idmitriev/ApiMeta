using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ApiMeta.Common.Models
{
    [DataContract(Name = "doc")]
    public class DocumentationArticle: IEntity<String>
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "text")]
        public String Text { get; set; }
    }
}
