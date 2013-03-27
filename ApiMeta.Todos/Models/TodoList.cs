using System;
using System.Runtime.Serialization;
using ApiMeta.Common.Attributes;
using ApiMeta.Common.Models;

namespace ApiMeta.Todos.Models
{
    [Meta]
    [DataContract(Name = "TodoList")]
    public class TodoList : IAuthoredEntity<Int64>
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "author")]
        public string Author { get; set; }

        [DataMember(Name = "name")]
        public String Name { get; set; }
    }
}