using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Shipping.ShipRocket
{
    public partial class RouteProvider : IRouteProvider
    {
        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="endpointRouteBuilder">Route builder</param>
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            //Webhook
            endpointRouteBuilder.MapControllerRoute("Plugin.Payments.ShipRocket.WebhookHandler", "Plugins/ShipRocket/Webhook",
                new { controller = "ShipRocket", action = "Webhook" });
        }

        public int Priority => 0;
    }
}
