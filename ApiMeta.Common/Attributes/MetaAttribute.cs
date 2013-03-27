using System;

namespace ApiMeta.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class MetaAttribute : Attribute
    {
        
    }
}