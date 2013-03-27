using System;
using System.Runtime.Serialization;
using ApiMeta.Common.Attributes;
using ApiMeta.Common.Models;

namespace ApiMeta.Todos.Models
{
    [Meta]
    [DataContract(Name = "Todo")]
    public class Todo : IAuthoredEntity<Int64>
    {
        [DataMember(Name = "author")]
        public string Author { get; set; }

        [DataMember(Name = "text")]
        public String Text { get; set; }

        [DataMember(Name = "done")]
        public Boolean Done { get; set; }

        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "listId")]
        public Int64 ListId { get; set; }
    }
}