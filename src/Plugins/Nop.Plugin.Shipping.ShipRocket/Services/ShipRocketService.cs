using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Nop.Core.Caching;
using Nop.Plugin.Shipping.ShipRocket.Models;

namespace Nop.Plugin.Shipping.ShipRocket.Services
{
    public class ShipRocketService : IShipRocketService
    {
        private const string API_URL = "https://apiv2.shiprocket.in/v1/external/";
        private const string AUTH_URL = "auth/login/";
        private const string CREATE_ORDER_URL = "/orders/create/adhoc";
        private const string CONTENT_TYPE = "application/json";
        private readonly CacheKey _serviceCacheKey = new CacheKey("Nop.plugins.shipping.shiprocket.servicecachekey.{0}");

        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ShipRocketSettings _shipRocketSettings;

        public ShipRocketService(IStaticCacheManager staticCacheManager, ShipRocketSettings shipRocketSettings)
        {
            _staticCacheManager = staticCacheManager;
            _shipRocketSettings = shipRocketSettings;
        }

        public string GetToken()
        {
            var cacheKey = _staticCacheManager.PrepareKeyForShortTermCache(_serviceCacheKey, "token");

            var token = _staticCacheManager.Get<string>(cacheKey, () => UpdateToken());

            return token;
        }

        public string UpdateToken()
        {
            var webResponse = SendPostRequest($"{API_URL}{AUTH_URL}", new { email = _shipRocketSettings.Email, password = _shipRocketSettings.Password });

            var tokenResponse = JsonConvert.DeserializeObject<ShipRocketTokenResponse>(webResponse);

            return tokenResponse.Token;
        }

        public ShipRocketOrderResponse CreateOrder(ShipRocketOrder shipRocketOrder)
        {
            var webResponse = SendPostRequest($"{API_URL}{CREATE_ORDER_URL}", shipRocketOrder, true);

            var createOrderResponse = JsonConvert.DeserializeObject<ShipRocketOrderResponse>(webResponse);

            return createOrderResponse;
        }

        protected virtual string SendPostRequest(string url, object postData, bool includeToken = false)
        {
            var response = string.Empty;
            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", CONTENT_TYPE);

                if (includeToken)
                {
                    client.Headers.Add("Authorization", "Bearer " + GetToken());
                }

                response = client.UploadString(url, JsonConvert.SerializeObject(postData));
            }

            return response;
        }
    }
}
