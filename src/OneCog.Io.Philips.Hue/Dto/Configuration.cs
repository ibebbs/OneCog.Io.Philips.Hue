using Newtonsoft.Json;
using System;

namespace OneCog.Io.Philips.Hue.Dto
{
    public class Configuration
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("mac")]
        public string Mac { get; set; }

        [JsonProperty("dhcp")]
        public bool Dhcp { get; set; }

        [JsonProperty("ipaddress")]
        public string IpAddress { get; set; }

        [JsonProperty("netmask")]
        public string Netmask { get; set; }

        [JsonProperty("gateway")]
        public string Gateway { get; set; }

        [JsonProperty("proxyaddress")]
        public string ProxyAddress { get; set; }

        [JsonProperty("proxyport")]
        public int ProxyPort { get; set; }

        [JsonProperty("UTC")]
        public DateTime Utc { get; set; }

        [JsonProperty("whitelist")]
        public Whitelist Whitelist { get; set; }

        [JsonProperty("swversion")]
        public string SoftwareVersion { get; set; }

        [JsonProperty("swupdate")]
        public SoftwareUpdate SoftwareUpdate { get; set; }

        [JsonProperty("linkbutton")]
        public bool LinkButton { get; set; }

        [JsonProperty("PortalServices")]
        public bool portalservices { get; set; }
    }
}
