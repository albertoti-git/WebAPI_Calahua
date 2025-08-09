using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using WebApiRESTv1.Repositories; // Asegúrate de tener este using para tus interfaces/repositorios

namespace WebApiRESTv1
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var config = GlobalConfiguration.Configuration;

            // Configuración de Autofac
            var builder = new ContainerBuilder();

            // Registrar controladores Web API
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Registrar dependencias (ejemplo: ContactRepository)
            builder.RegisterType<ContactRepository>().As<IContactRepository>().InstancePerRequest();

            // Si tienes otras dependencias, agrégalas aquí
            // builder.RegisterType<MiServicio>().As<IMiServicio>();

            // Construir el contenedor
            var container = builder.Build();

            // Establecer el resolver de dependencias de Autofac
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // Resto de configuraciones
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(System.Web.Mvc.GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(System.Web.Routing.RouteTable.Routes);
            BundleConfig.RegisterBundles(System.Web.Optimization.BundleTable.Bundles);
        }
    }
}
