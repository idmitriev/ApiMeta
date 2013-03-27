using System.Collections.ObjectModel;
using System.Net.Http;
using ApiMeta.Common.Handlers;

namespace ApiMeta.Todos.App_Start
{
    public class HandlersConfig
    {
        public static void RegisterHandlers(Collection<DelegatingHandler> messageHandlers)
        {
            messageHandlers.Add(new CORSHandler());
        }
    }
}