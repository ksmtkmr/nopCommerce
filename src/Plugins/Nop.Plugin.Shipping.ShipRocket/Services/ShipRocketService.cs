using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Nop.Core.Caching;

namespace Nop.Plugin.Shipping.ShipRocket.Services
{
    public class ShipRocketService : IShipRocketService
    {
        private const string API_URL = "https://apiv2.shiprocket.in/v1/external/";
        private const string AUTH_URL = "auth/login/";
        private readonly CacheKey _serviceCacheKey = new CacheKey("Nop.plugins.shipping.shiprocket.servicecachekey.{0}");

        private readonly IStaticCacheManager _staticCacheManager;
        private readonly ShipRocketSettings _shipRocketSettings;

        public ShipRocketService(IStaticCacheManager staticCacheManager, ShipRocketSettings shipRocketSettings)
        {
            _staticCacheManager = staticCacheManager;
            _shipRocketSettings = shipRocketSettings;
        }

        string GetToken()
        {
            var cacheKey = _staticCacheManager.PrepareKeyForShortTermCache(_serviceCacheKey, "token");

            var token = _staticCacheManager.Get<string>(cacheKey, () => SendGetRequest(string.Format($"{API_URL}{AUTH_URL}")));
            return "";
        }

        protected virtual string SendGetRequest(string apiUrl)
        {
            var request = WebRequest.Create(apiUrl);

            request.Credentials = new NetworkCredential(_shipRocketSettings.Email, _shipRocketSettings.Password);
            var resp = request.GetResponse();

            using var rs = resp.GetResponseStream();
            if (rs == null)
                return string.Empty;
            using var sr = new StreamReader(rs);
            return sr.ReadToEnd();
        }
    }
}
