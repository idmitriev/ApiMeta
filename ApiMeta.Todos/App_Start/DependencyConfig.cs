using System;
using System.Web.Http;
using ApiMeta.Common.Controllers;
using ApiMeta.Common.Data;
using ApiMeta.Common.Filters;
using ApiMeta.Common.Membership;
using ApiMeta.Common.Models;
using System.Reflection;
using ApiMeta.Todos.Models;
using Autofac;
using Autofac.Integration.WebApi;

namespace ApiMeta.Todos.App_Start
{
    public class DependencyConfig
    {
        public static void Bind(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(new [] { Assembly.GetExecutingAssembly(), typeof(DocumentationController).Assembly});

            builder.RegisterType<InMemRepository<DocumentationArticle, String>>().As<IRepository<DocumentationArticle, String>>().SingleInstance();
            builder.RegisterType<InMemRepository<Todo, Int64>>().As<IRepository<Todo, Int64>>().SingleInstance();
            builder.RegisterType<InMemRepository<TodoList, Int64>>().As<IRepository<TodoList, Int64>>().SingleInstance();
            
            builder.Register(c => new TypeMetadataController(typeof(Todo).Assembly, new [] { typeof(Todo).Namespace })).As<TypeMetadataController>().InstancePerApiRequest();
            builder.Register(c => new ResourceMetadataController(config.Services.GetApiExplorer())).InstancePerApiRequest();

            builder.RegisterType<FakeMembershipValidator>().As<IMembershipValidator>().InstancePerLifetimeScope();
            builder.RegisterType<BasicAuthenticationRequiredAttribute>().OnActivating(e => e.Instance.MembershipValidator = e.Context.Resolve<IMembershipValidator>());

            builder.RegisterWebApiFilterProvider(config);

            var container = builder.Build();
            
            var resolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }
    }
}