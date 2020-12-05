using System;
using System.Collections.Generic;
using System.Text;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Shipping.ShipRocket.Models
{
    public class ShipRocketConfiguration : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugins.Shipping.ShipRocket.Fields.Email")]
        public string Email { get; set; }
        public bool Email_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Shipping.ShipRocket.Fields.Password")]
        public string Password { get; set; }
        public bool Password_OverrideForStore { get; set; }
    }
}
