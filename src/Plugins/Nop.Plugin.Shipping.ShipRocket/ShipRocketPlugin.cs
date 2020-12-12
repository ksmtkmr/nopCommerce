using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Domain.Shipping;
using Nop.Plugin.Shipping.ShipRocket.Services;
using Nop.Services.Common;
using Nop.Services.Plugins;
using Nop.Services.Shipping;
using Nop.Services.Shipping.Tracking;

namespace Nop.Plugin.Shipping.ShipRocket
{
    public class ShipRocketPlugin : BasePlugin, IShippingRateComputationMethod, IMiscPlugin
    {
        private readonly IShipRocketService _shipRocketService;

        public ShipRocketPlugin(IShipRocketService shipRocketService)
        {
            _shipRocketService = shipRocketService;
        }

        public ShippingRateComputationMethodType ShippingRateComputationMethodType => ShippingRateComputationMethodType.Realtime;

        public IShipmentTracker ShipmentTracker => throw new NotImplementedException();

        public decimal? GetFixedRate(GetShippingOptionRequest getShippingOptionRequest)
        {
            return 10;
        }

        public GetShippingOptionResponse GetShippingOptions(GetShippingOptionRequest getShippingOptionRequest)
        {
            var shippingOptions = new GetShippingOptionResponse
            {
                ShippingFromMultipleLocations = true,
                ShippingOptions = new List<ShippingOption>
                {
                    new ShippingOption
                    {
                        Name = "ShipRocket",
                        IsPickupInStore = false,
                    }
                }
            };

            return shippingOptions;
        }

        public override void Install()
        {
            base.Install();
        }

        public override void Uninstall()
        {
            base.Uninstall();
        }
    }
}
