
using Newtonsoft.Json;

namespace mTerms_Proxy_api
{
    public struct ConfigJson
    {
        [JsonProperty("Link")]
        public string Link { get; private set; }
        [JsonProperty("Username")]
        public string Username { get; private set; }
        [JsonProperty("Password")]
        public string Password { get; private set; }
    }
}
