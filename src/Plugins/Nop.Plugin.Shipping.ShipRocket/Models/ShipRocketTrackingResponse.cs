using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Nop.Plugin.Shipping.ShipRocket.Models
{
    public class ShipRocketTrackingResponse
    {
        [JsonProperty("tracking_data")]
        public TrackingData TrackingData { get; set; }
    }

    public class TrackingData
    {
        [JsonProperty("track_status")]
        public int TrackStatus { get; set; }

        [JsonProperty("shipment_status")]
        public int ShipmentStatus { get; set; }

        [JsonProperty("shipment_track")]
        public List<ShipmentTrack> ShipmentTrack { get; set; }

        [JsonProperty("shipment_track_activities")]
        public List<ShipmentTrackActivity> ShipmentTrackActivities { get; set; }

        [JsonProperty("track_url")]
        public string TrackUrl { get; set; }
    }

    public class ShipmentTrack
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("awb_code")]
        public string AwbCode { get; set; }

        [JsonProperty("courier_company_id")]
        public int CourierCompanyId { get; set; }

        [JsonProperty("shipment_id")]
        public string ShipmentId { get; set; }

        [JsonProperty("order_id")]
        public int OrderId { get; set; }

        [JsonProperty("pickup_date")]
        public object PickupDate { get; set; }

        [JsonProperty("delivered_date")]
        public string DeliveredDate { get; set; }

        [JsonProperty("weight")]
        public string Weight { get; set; }

        [JsonProperty("packages")]
        public int Packages { get; set; }

        [JsonProperty("current_status")]
        public string CurrentStatus { get; set; }

        [JsonProperty("delivered_to")]
        public string DeliveredTo { get; set; }

        [JsonProperty("destination")]
        public string Destination { get; set; }

        [JsonProperty("consignee_name")]
        public string ConsigneeName { get; set; }

        [JsonProperty("origin")]
        public string Origin { get; set; }

        [JsonProperty("courier_agent_details")]
        public string CourierAgentDetails { get; set; }
    }

    public class ShipmentTrackActivity
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("activity")]
        public string Activity { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }
    }

    
}
