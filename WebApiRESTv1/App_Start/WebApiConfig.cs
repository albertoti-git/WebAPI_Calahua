using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using WebApiRESTv1.Models;
using WebApiRESTv1.Controllers;
using WebApiRESTv1.Repositories;
using WebApiRESTv1.DependencyInjection;
using WebApiRESTv1.Repositories.Price;
using WebApiRESTv1.Repositories.Quotation;
using WebApiRESTv1.Repositories.Invoice;

namespace WebApiRESTv1
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración y servicios de API web

            // Rutas de API web
            config.MapHttpAttributeRoutes();

            config.MessageHandlers.Add(new TokenValidationHandler());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            Models.ConexionSAP conexionSAP = ConexionSAP.GetInstance;
            conexionSAP.Servidor = ConfigurationManager.AppSettings["Servidor"];
            conexionSAP.Catalogo = ConfigurationManager.AppSettings["Catalogo"];
            conexionSAP.UsuarioSAP = ConfigurationManager.AppSettings["UsuarioSAP"];
            conexionSAP.ContrasenaSBO = ConfigurationManager.AppSettings["ContrasenaSBO"];
            conexionSAP.TipoServidor = ConfigurationManager.AppSettings["TipoServidor"];
            conexionSAP.Conectar();

            var orderDiApiRepository = new OrderDiApiRepository(conexionSAP.CompanySBO);
            var orderRepository = new OrderRepository();
            var contactRepository = new ContactRepository();
            var priceRepository = new PriceRepository();
            var quoteRepository = new QuoteRepository();
            var invoiceRepository = new InvoiceRepository();
            config.DependencyResolver = new SimpleResolver(
                orderDiApiRepository,
                orderRepository,
                contactRepository,
                priceRepository,
                quoteRepository,
                invoiceRepository
                );
        }
    }
}
