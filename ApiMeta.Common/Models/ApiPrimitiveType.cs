using System.Runtime.Serialization;

namespace ApiMeta.Common.Models
{
    public enum ApiPrimitiveType
    {
        [EnumMember(Value = "null")]
        Null,
        [EnumMember(Value = "boolean")]
        Boolean,
        [EnumMember(Value = "string")]
        String,
        [EnumMember(Value = "number")]
        Number
    }

    public enum JsType
    {
        [EnumMember(Value = "date")]
        Date
    }
}
