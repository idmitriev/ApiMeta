using System.Web.Http;
using ApiMeta.Todos.App_Start;

namespace ApiMeta.Todos
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalConfiguration.Configuration.Filters);
            DependencyConfig.Bind(GlobalConfiguration.Configuration);
            FormattersConfig.RegisterFormatters(GlobalConfiguration.Configuration.Formatters);
            HandlersConfig.RegisterHandlers(GlobalConfiguration.Configuration.MessageHandlers);
        }
    }
}