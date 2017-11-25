
using System;
using System.Net;
using System.Collections.Generic;

using Newtonsoft.Json;


namespace stratfaucet
{
        public partial class Captcha
        {

            [JsonProperty("secret")]
            public string Secret { get; set; }

            [JsonProperty("response")]
            public string Response { get; set; }

            [JsonProperty("remoteip")]
            public string RemoteIP { get; set; }

        }
}
