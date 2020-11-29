using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Configuration;

namespace Nop.Plugin.Shipping.ShipRocket
{
    public class ShipRocketSettings : ISettings
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
