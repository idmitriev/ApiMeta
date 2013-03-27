using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ApiMeta.Common.Models
{
    [DataContract(Name = "type")]
    public class ApiType : DocumentedEntity
    {
        [DataMember(Name = "name")]
        public String Name { get; set; }

        [DataMember(Name = "properties")]
        public IList<ApiTypeProperty> Properties { get; set; }

        public static String GetTypeName(Type type)
        {
            if (type == null)
                return ApiPrimitiveType.Null.ToString();

            if (new[] { typeof(String), typeof(Guid) }.Contains(type))
                return ApiPrimitiveType.String.ToString();

            if (new[] { typeof(Int32), typeof(Int64), typeof(Decimal), typeof(Byte), typeof(Double) }.Contains(type))
                return ApiPrimitiveType.Number.ToString();

            if (new[] { typeof(DateTime), typeof(TimeSpan) }.Contains(type))
                return JsType.Date.ToString();

            var nullableUnderlaying = Nullable.GetUnderlyingType(type);

            if (nullableUnderlaying != null)
                return GetTypeName(nullableUnderlaying);

            if (type.IsGenericType && type.GetInterfaces().Contains(typeof(IEnumerable)) && type.GetGenericArguments().Any())
                return String.Format("{0}[]", GetTypeName(type.GetGenericArguments()[0]));

            if (type.Name.ToString() == "HttpResponseMessage")
                return ApiPrimitiveType.Null.ToString();

            return type.Name.ToString();
        }
    }
}
