using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Plugin.Shipping.ShipRocket.Services;

namespace Nop.Plugin.Shipping.ShipRocket.Infrastructure
{
    class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => 3;

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, AppSettings appSettings)
        {
            builder.RegisterType<ShipRocketService>().As<IShipRocketService>().InstancePerLifetimeScope();
        }
    }
}
