using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Nop.Plugin.Shipping.ShipRocket.Services;
using Nop.Services.Shipping.Tracking;

namespace Nop.Plugin.Shipping.ShipRocket
{
    public class ShipRocketShipmentTracker : IShipmentTracker
    {
        private readonly IShipRocketService _shipRocketService;

        public ShipRocketShipmentTracker(IShipRocketService shipRocketService)
        {
            _shipRocketService = shipRocketService;
        }

        public IList<ShipmentStatusEvent> GetShipmentEvents(string trackingNumber)
        {
            var shipmentEvents = new List<ShipmentStatusEvent>();

            var shipmentTrackerAwaiter = _shipRocketService.GetShipmentTracking(trackingNumber);

            shipmentTrackerAwaiter.Wait();

            var shipmentTrackerResponse = shipmentTrackerAwaiter.Result;

            shipmentEvents.AddRange(shipmentTrackerResponse.TrackingData.ShipmentTrackActivities.Select(x => new ShipmentStatusEvent
            {
                Date = DateTime.Parse(x.Date),
                EventName = x.Activity,
                Location = x.Location,
            }).ToList());

            return shipmentEvents;
        }

        public string GetUrl(string trackingNumber)
        {
            return $"https://apiv2.shiprocket.in/v1/external/courier/track/awb/{trackingNumber}";
        }

        public bool IsMatch(string trackingNumber)
        {
            if (string.IsNullOrEmpty(trackingNumber))
                return false;

            return Regex.IsMatch(trackingNumber, "^\\d+$", RegexOptions.IgnoreCase);
        }
    }
}
