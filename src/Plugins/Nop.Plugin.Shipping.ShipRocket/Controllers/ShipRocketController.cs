using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Shipping.ShipRocket.Models;
using Nop.Plugin.Shipping.ShipRocket.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Shipping.ShipRocket.Controllers
{
    public class ShipRocketController : BasePluginController
    {
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly ISettingService _settingService;
        private readonly IShipRocketService _shipRocketService;
        private readonly IStoreContext _storeContext;
        private readonly IWebHelper _webHelper;

        public ShipRocketController(ILocalizationService localizationService,
            INotificationService notificationService,
            ISettingService settingService,
            IShipRocketService shipRocketService,
            IStoreContext storeContext,
            IWebHelper webHelper)
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _settingService = settingService;
            _shipRocketService = shipRocketService;
            _storeContext = storeContext;
            _webHelper = webHelper;
        }

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public ActionResult Configure()
        {
            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var shipStationSettings = _settingService.LoadSetting<ShipRocketSettings>(storeScope);

            var model = new ShipRocketConfiguration
            {
                
                ActiveStoreScopeConfiguration = storeScope,
                Email = shipStationSettings.Email,
                Password = shipStationSettings.Password,
            };

            if (storeScope <= 0)
                return View("~/Plugins/Shipping.ShipRocket/Views/Configure.cshtml", model);

            model.Password_OverrideForStore = _settingService.SettingExists(shipStationSettings, x => x.Password, storeScope);
            model.Email_OverrideForStore = _settingService.SettingExists(shipStationSettings, x => x.Email, storeScope);

            return View("~/Plugins/Shipping.ShipRocket/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public ActionResult Configure(ShipRocketConfiguration model)
        {
            if (!ModelState.IsValid)
                return Configure();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var shipStationSettings = _settingService.LoadSetting<ShipRocketSettings>(storeScope);

            //save settings
            shipStationSettings.Password = model.Password;
            shipStationSettings.Email = model.Email;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            _settingService.SaveSettingOverridablePerStore(shipStationSettings, x => x.Password, model.Password_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(shipStationSettings, x => x.Email, model.Email_OverrideForStore, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }
    }
}
