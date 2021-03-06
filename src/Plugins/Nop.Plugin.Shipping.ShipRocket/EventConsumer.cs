﻿
using System.Linq;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Shipping;
using Nop.Core.Infrastructure;
using Nop.Plugin.Shipping.ShipRocket.Models;
using Nop.Plugin.Shipping.ShipRocket.Services;
using Nop.Services.Common;
using Nop.Services.Directory;
using Nop.Services.Events;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Shipping;
using static Nop.Services.Shipping.GetShippingOptionRequest;

namespace Nop.Plugin.Shipping.ShipRocket
{
    public class EventConsumer : IConsumer<OrderPlacedEvent>
    {
        private readonly IAddressService _addressService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly ICountryService _countryService;
        private readonly IShipRocketService _shipRocketService;
        private readonly IPaymentService _paymentService;
        private readonly IShipmentService _shipmentService;
        private readonly IOrderService _orderService;

        public EventConsumer(
            IAddressService addressService,
            ICountryService countryService,
            IStateProvinceService stateProvinceService,
            IShipRocketService shipRocketService,
            IPaymentService paymentService,
            IOrderService orderService,
            IShipmentService shipmentService)
        {
            _addressService = addressService;
            _countryService = countryService;
            _stateProvinceService = stateProvinceService;
            _shipRocketService = shipRocketService;
            _paymentService = paymentService;
            _orderService = orderService;
            _shipmentService = shipmentService;
        }

        /// <summary>
        /// Executes code when user places order
        /// </summary>
        /// <param name="eventMessage"></param>
        public void HandleEvent(OrderPlacedEvent eventMessage)
        {
            var order = eventMessage.Order;

            var orderItems = _orderService.GetOrderItems(order.Id);

            var billingAddress = _addressService.GetAddressById(order.BillingAddressId);

            var billingCountryName = string.Empty;
            if (billingAddress.CountryId.HasValue)
            {
                var billingCountry = _countryService.GetCountryById(billingAddress.CountryId.Value);
                billingCountryName = billingCountry.Name;
            }

            var billingStateName = string.Empty;
            if (billingAddress.StateProvinceId.HasValue)
            {
                var billingState = _stateProvinceService.GetStateProvinceById(billingAddress.CountryId.Value);
                billingStateName = billingState.Name;
            }

            for (var i = 0; i < orderItems.Count; i++)
            {
                var product = _orderService.GetProductByOrderItemId(orderItems[i].Id);

                var shipRocketOrder = new ShipRocketOrder
                {
                    BillingAddress = billingAddress.Address1,
                    BillingAddress2 = billingAddress.Address2,
                    BillingPhoneNumber = billingAddress.PhoneNumber,
                    BillingCity = billingAddress.City,
                    BillingCountry = billingCountryName,
                    BillingCustomerFirstName = billingAddress.FirstName,
                    BillingCustomerLastName = billingAddress.LastName,
                    BillingEmail = billingAddress.Email,
                    BillingPinCode = billingAddress.ZipPostalCode,
                    BillingState = billingStateName,
                    ProductSKU = product.Sku,
                    ProductWeight = product.Weight,
                    ProductHeight = product.Height,
                    ProductLength = product.Length,
                    ProductBreadth = product.Width
                };

                if (order.ShippingAddressId.HasValue)
                {
                    var shippingAddress = _addressService.GetAddressById(order.ShippingAddressId.Value);

                    var shippingCountryName = string.Empty;
                    if (shippingAddress.CountryId.HasValue)
                    {
                        var shippingCountry = _countryService.GetCountryById(shippingAddress.CountryId.Value);
                        shippingCountryName = shippingCountry.Name;
                    }

                    var shippingStateName = string.Empty;
                    if (shippingAddress.StateProvinceId.HasValue)
                    {
                        var shippingState = _stateProvinceService.GetStateProvinceById(shippingAddress.CountryId.Value);
                        shippingStateName = shippingState.Name;
                    }

                    shipRocketOrder.IsShippingAddressSameAsBilling = false;

                    shipRocketOrder.ShippingAddress = shippingAddress.Address1;
                    shipRocketOrder.ShippingAddress2 = shippingAddress.Address2;
                    shipRocketOrder.ShippingCity = shippingAddress.City;
                    shipRocketOrder.ShippingCountry = shippingCountryName;
                    shipRocketOrder.ShippingState = shippingStateName;
                    shipRocketOrder.ShippingCustomerFirstName = shippingAddress.FirstName;
                    shipRocketOrder.ShippingCustomerLastName = shippingAddress.LastName;
                    shipRocketOrder.ShippingEmail = shippingAddress.Email;
                    shipRocketOrder.ShippingPhone = shippingAddress.PhoneNumber;
                    shipRocketOrder.ShippingPinCode = shippingAddress.ZipPostalCode;

                    //To Do: Set shipRocketOrder.TransactionCharges
                }
                else
                {
                    shipRocketOrder.IsShippingAddressSameAsBilling = true;
                }

                shipRocketOrder.SubTotal = orderItems[i].PriceExclTax;
                shipRocketOrder.TaxPercentage = orderItems[i].PriceInclTax - orderItems[i].PriceExclTax;
                shipRocketOrder.TotalDiscount = orderItems[i].DiscountAmountExclTax;

                var shipRocketOrderResponse = Task.Run(() => _shipRocketService.CreateOrder(shipRocketOrder)).Result;

                var shipment = new Shipment
                {
                    OrderId = order.Id,
                    TrackingNumber = shipRocketOrderResponse.AwbCode,
                };
            }
        }
    }
}
