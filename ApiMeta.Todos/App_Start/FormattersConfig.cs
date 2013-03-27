using System.Linq;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Converters;

namespace ApiMeta.Todos.App_Start
{
    public class FormattersConfig
    {
        public static void RegisterFormatters(MediaTypeFormatterCollection config)
        {
            config.Remove(config.XmlFormatter);

            config.OfType<JsonMediaTypeFormatter>()
                .First()
                .SerializerSettings
                .Converters
                .Add(new StringEnumConverter());
        }
    }
}