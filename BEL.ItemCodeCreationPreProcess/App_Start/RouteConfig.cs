namespace BEL.ItemCodeCreationPreProcess
{
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// Route Config
    /// </summary>
    public static class RouteConfig
    { 
        /// <summary>
        /// Registers the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "ItemCodeCreation", action = "Index", id = UrlParameter.Optional });
        }
    }
}
