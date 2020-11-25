using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace sales.Model.util
{
    public class TokenResponse
    {
        [JsonProperty(PropertyName = "token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "user")]
        public UserRequest UserRequest  { get; set; }
    }
}
