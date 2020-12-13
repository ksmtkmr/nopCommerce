using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core;
using Nop.Core.Domain.Shipping;
using Nop.Plugin.Shipping.ShipRocket.Services;
using Nop.Services.Common;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Shipping;
using Nop.Services.Shipping.Tracking;

namespace Nop.Plugin.Shipping.ShipRocket
{
    public class ShipRocketPlugin : BasePlugin, IShippingRateComputationMethod, IMiscPlugin
    {
        private readonly IShipRocketService _shipRocketService;
        private readonly IWebHelper _webHelper;
        private readonly ILocalizationService _localizationService;

        public ShipRocketPlugin(IShipRocketService shipRocketService, IWebHelper webHelper, ILocalizationService localizationService)
        {
            _shipRocketService = shipRocketService;
            _webHelper = webHelper;
            _localizationService = localizationService;
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

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/ShipRocket/Configure";
        }

        public override void Install()
        {
            //locales
            _localizationService.AddLocaleResource(new Dictionary<string, string>
            {
                ["Plugins.Shipping.ShipRocket.Fields.Password.Hint"] = "Specify ShipRocket API User's password",
                ["Plugins.Shipping.ShipRocket.Fields.Password"] = "Password",
                ["Plugins.Shipping.ShipRocket.Fields.Email"] = "Email",
                ["Plugins.Shipping.ShipRocket.Fields.Email.Hint"] = "Specify ShipRocket API User's Email"
            });

            base.Install();
        }

        public override void Uninstall()
        {
            _localizationService.DeleteLocaleResources("Plugins.Shipping.ShipRocket");

            base.Uninstall();
        }
    }
}
