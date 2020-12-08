using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nop.Core.Caching;
using Nop.Plugin.Shipping.ShipRocket.Models;

namespace Nop.Plugin.Shipping.ShipRocket.Services
{
    public class ShipRocketService : IShipRocketService
    {
        //Variable declarations
        private const string API_URL = "https://apiv2.shiprocket.in/v1/external/";
        private const string AUTH_URL = "auth/login/";
        private const string CREATE_ORDER_URL = "orders/create/adhoc";
        private const string CREATE_CHANNEL_SPECIFIC_ORDER_URL = "orders/create";
        private const string UPDATE_PICKUP_LOCATION_URL = "orders/address/pickup";
        private const string UPDATE_CUSTOMER_ADDRESS_URL = "orders/address/update";
        private const string CANCEL_ORDER_URL = "orders/cancel";
        private const string COURIER_TRACKING_URL = "courier/track/awb/{0}";
        private const string CONTENT_TYPE = "application/json";
        private readonly CacheKey _serviceCacheKey = new CacheKey("Nop.plugins.shipping.shiprocket.servicecachekey.{0}");

        //Services declaration
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ShipRocketSettings _shipRocketSettings;

        public ShipRocketService(IStaticCacheManager staticCacheManager, ShipRocketSettings shipRocketSettings)
        {
            _staticCacheManager = staticCacheManager;
            _shipRocketSettings = shipRocketSettings;
        }

        /// <summary>
        /// Get Ship Rocket Token
        /// </summary>
        /// <returns></returns>
        public string GetToken()
        {
            var cacheKey = _staticCacheManager.PrepareKeyForShortTermCache(_serviceCacheKey, "token");

            var token = _staticCacheManager.Get(cacheKey, () => UpdateToken());

            return token;
        }

        /// <summary>
        /// Fetch Token from Ship Rocket
        /// </summary>
        /// <returns></returns>
        public string UpdateToken()
        {
            var webResponse = string.Empty;

            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", CONTENT_TYPE);

                webResponse = client.UploadString($"{API_URL}{AUTH_URL}", "POST", JsonConvert.SerializeObject(new { email = _shipRocketSettings.Email, password = _shipRocketSettings.Password }));
            }

            var tokenResponse = JsonConvert.DeserializeObject<ShipRocketTokenResponse>(webResponse);

            return tokenResponse.Token;
        }

        /// <summary>
        /// create a quick custom order. Quick orders are the ones where ShipRocket does not store the product details in the master catalog. 
        /// </summary>
        /// <param name="shipRocketOrder"></param>
        /// <returns></returns>
        public async Task<ShipRocketOrderResponse> CreateOrder(ShipRocketOrder shipRocketOrder)
        {
            var webResponse = await SendPostRequest($"{API_URL}{CREATE_ORDER_URL}", shipRocketOrder, true);

            var createOrderResponse = JsonConvert.DeserializeObject<ShipRocketOrderResponse>(webResponse.Data);

            return createOrderResponse;
        }

        /// <summary>
        /// create a quick custom order. Quick orders are the ones where ShipRocket does not store the product details in the master catalog. 
        /// </summary>
        /// <param name="shipRocketOrder"></param>
        /// <returns></returns>
        public async Task<ShipRocketOrderResponse> CreateChannelSpecificOrder(ShipRocketOrder shipRocketOrder)
        {
            if (shipRocketOrder.ChannelId == null)
            {
                return null;
            }
            var webResponse = await SendPostRequest($"{API_URL}{CREATE_CHANNEL_SPECIFIC_ORDER_URL}", shipRocketOrder, true);

            var createOrderResponse = JsonConvert.DeserializeObject<ShipRocketOrderResponse>(webResponse.Data);

            return createOrderResponse;
        }

        /// <summary>
        /// Updates pickup location of an existing order
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="pickupLocation"></param>
        /// <returns></returns>
        public async Task<ShipRocketPickupLocationResponse> UpdatePickupLocation(int? orderId, string pickupLocation)
        {
            if (orderId == null || pickupLocation == null)
            {
                return null;
            }

            var webResponse = await SendPatchRequest($"{API_URL}{UPDATE_PICKUP_LOCATION_URL}", new { order_id = orderId, pickup_location = pickupLocation }, true);

            var pickupLocationResponse = JsonConvert.DeserializeObject<ShipRocketPickupLocationResponse>(webResponse.Data);

            return pickupLocationResponse;
        }

        /// <summary>
        /// create a quick custom order. Quick orders are the ones where ShipRocket does not store the product details in the master catalog. 
        /// </summary>
        /// <param name="shipRocketOrder"></param>
        /// <returns></returns>
        public async Task<bool> UpdateCustomerDeliveryAddress(ShipRocketOrder shipRocketOrder)
        {
            var webResponse = await SendPostRequest($"{API_URL}{UPDATE_CUSTOMER_ADDRESS_URL}", shipRocketOrder, true);

            return webResponse.StatusCode == HttpStatusCode.Accepted;
        }

        /// <summary>
        /// Cancels all orders having the specified order ids
        /// </summary>
        /// <param name="orderIds"></param>
        /// <returns></returns>
        public async Task<bool> CancelOrder(List<int> orderIds)
        {
            var webResponse = await SendPostRequest($"{API_URL}{CANCEL_ORDER_URL}", orderIds, true);

            return webResponse.StatusCode == HttpStatusCode.NoContent;
        }


        /// <summary>
        /// Get Shipment Tracking
        /// </summary>
        /// <param name="AwbCode"></param>
        /// <returns></returns>
        public async Task<ShipRocketTrackingResponse> GetShipmentTracking(string awbCode)
        {
            var webResponse = await SendGetRequest(string.Format($"{API_URL}{COURIER_TRACKING_URL}", awbCode), null, true);

            var shipRocketTracking = JsonConvert.DeserializeObject<ShipRocketTrackingResponse>(webResponse.Data);

            return shipRocketTracking;
        }

        #region Private Methods

        /// <summary>
        /// Allows sending a GET Request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="queryParamKeyPairs"></param>
        /// <param name="includeToken"></param>
        /// <returns></returns>
        private async Task<GenericResponse> SendGetRequest(string url, List<KeyValuePair<string,string>> queryParamKeyPairs, bool includeToken = false)
        {
            var httpClient = new HttpClient();

            var response = new GenericResponse();

            if (includeToken)
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetToken());
            }

            var queryParams = string.Empty;

            if (queryParamKeyPairs != null)
            {
                using var content = new FormUrlEncodedContent(queryParamKeyPairs);

                queryParams = content.ReadAsStringAsync().Result;
            }

            var httpResponse = await httpClient.GetAsync(url+ queryParams);

            response.Data = await httpResponse.Content.ReadAsStringAsync();

            response.StatusCode = httpResponse.StatusCode;

            return response;
        }

        /// <summary>
        /// Allows posting requests to the passed url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="includeToken"></param>
        /// <returns></returns>
        private async Task<GenericResponse> SendPostRequest(string url, object postData, bool includeToken = false)
        {
            var httpClient = new HttpClient();

            var response = new GenericResponse();
            
            if (includeToken)
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetToken());
            }

            var content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, CONTENT_TYPE);

            var httpResponse = await httpClient.PostAsync(url, content);

            response.Data = await httpResponse.Content.ReadAsStringAsync();

            response.StatusCode = httpResponse.StatusCode;

            return response;
        }

        // <summary>
        /// Allows patch requests to the passed url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="includeToken"></param>
        /// <returns></returns>
        private async Task<GenericResponse> SendPatchRequest(string url, object postData, bool includeToken = false)
        {
            var httpClient = new HttpClient();

            var response = new GenericResponse();

            if (includeToken)
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetToken());
            }

            var content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, CONTENT_TYPE);

            var httpResponse = await httpClient.PatchAsync(url, content);

            response.Data = await httpResponse.Content.ReadAsStringAsync();

            response.StatusCode = httpResponse.StatusCode;

            return response;
        }

        #endregion
    }
}
