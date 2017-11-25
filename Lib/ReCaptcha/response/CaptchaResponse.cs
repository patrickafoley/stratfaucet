
using System;
using System.Net;
using System.Collections.Generic;

using Newtonsoft.Json;


namespace stratfaucet
{

        public partial class CaptchaResponse
        {

           [JsonProperty("success")]
            public bool Success { get; set; }


           [JsonProperty("challenge_ts")]
            public DateTime ChallengeTS { get; set; }


           [JsonProperty("hostname")]
            public string Hostname { get; set; }
        }

}
