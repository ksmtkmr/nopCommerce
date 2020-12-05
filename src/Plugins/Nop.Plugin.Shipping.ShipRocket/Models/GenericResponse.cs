using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Nop.Plugin.Shipping.ShipRocket.Models
{
    public class GenericResponse
    {
        public string Data { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}
