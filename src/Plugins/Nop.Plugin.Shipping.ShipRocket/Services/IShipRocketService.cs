using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.Shipping.ShipRocket.Models;

namespace Nop.Plugin.Shipping.ShipRocket.Services
{
    public interface IShipRocketService
    {
        Task<ShipRocketOrderResponse> CreateOrder(ShipRocketOrder shipRocketOrder);

        Task<ShipRocketOrderResponse> CreateChannelSpecificOrder(ShipRocketOrder shipRocketOrder);

        Task<ShipRocketPickupLocationResponse> UpdatePickupLocation(int? orderId, string pickupLocation);

        Task<bool> UpdateCustomerDeliveryAddress(ShipRocketOrder shipRocketOrder);

        Task<bool> CancelOrder(List<int> orderIds);

        Task<ShipRocketTrackingResponse> GetShipmentTracking(string awbCode);
    }
}
