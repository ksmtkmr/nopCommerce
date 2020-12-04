using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Nop.Plugin.Shipping.ShipRocket.Models
{
    public class ShipRocketOrderResponse
    {
        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("shipment_id")]
        public string ShipmentId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
        
        [JsonProperty("status_code")]
        public string StatusCode { get; set; }

        [JsonProperty("onboarding_completed_now")]
        public string IsOnboardingCompleted { get; set; }

        [JsonProperty("awb_code")]
        public string AwbCode { get; set; }

        [JsonProperty("courier_company_id")]
        public string CourierCompanyId { get; set; }

        [JsonProperty("courier_name")]
        public string CourierName { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("errors")]
        public object Errors { get; set; }
    }
}
