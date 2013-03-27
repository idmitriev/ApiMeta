using System.Web.Http;

namespace ApiMeta.Todos.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute( 
                name: "TodoListItems",
                routeTemplate: "list/{list}/todos",
                defaults: new { controller = "Todos" },
                constraints: new { list = @"\d+" }
            );
        }
    }
}
