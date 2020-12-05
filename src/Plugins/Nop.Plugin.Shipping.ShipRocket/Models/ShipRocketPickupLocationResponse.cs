using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Nop.Plugin.Shipping.ShipRocket.Models
{
    public class ShipRocketPickupLocationResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("status_code")]
        public string StatusCode { get; set; }
    }
}
