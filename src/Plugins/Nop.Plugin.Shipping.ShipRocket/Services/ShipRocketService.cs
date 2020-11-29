using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Nop.Core.Caching;

namespace Nop.Plugin.Shipping.ShipRocket.Services
{
    public class ShipRocketService : IShipRocketService
    {
        private const string API_URL = "https://apiv2.shiprocket.in/v1/external/";
        private const string AUTH_URL = "auth/login/";
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

            var token = _staticCacheManager.Get<string>(cacheKey, () =>
                SendPostRequest(string.Format($"{API_URL}{AUTH_URL}"),
                new { email = _shipRocketSettings.Email, password = _shipRocketSettings.Password }));

            return token;
        }

        protected virtual string SendPostRequest(string url, object postData)
        {
            var response = string.Empty;
            using (var client = new WebClient())
            {

                client.Headers.Add("Content-Type", CONTENT_TYPE);

                response = client.UploadString(url, JsonConvert.SerializeObject(postData));
            }

            return response;
        }
    }
}
