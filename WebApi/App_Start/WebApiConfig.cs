using Database;
using Ioc;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using WebApi.Controllers;
using WebApi.Filters;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Register controllers in the activartor
            //In reality this should be done by IoC container
            var drinkService = new DrinkService();
            var activator = new SimpleHttpControllerActivator();
            activator.Register(() => new CartController(drinkService));
            activator.Register(() => new DrinkController(drinkService));
            config.Services.Replace(typeof(IHttpControllerActivator), activator);

            //Register routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(name: "Cart", routeTemplate: "api/cart", defaults: new { controller = "Cart" });
            config.Routes.MapHttpRoute(name: "Drink", routeTemplate: "api/cart/drink/{name}", defaults: new { controller = "Drink" });

            //Register filters
            config.Filters.Add(new SimpleAuthorizationFilter());
            config.Filters.Add(new ModelVaidationFilter());
        }
    }
}
