using Newtonsoft.Json;
using System;

namespace OneCog.Io.Philips.Hue.Dto
{
    public class Developer
    {
        [JsonProperty("lastusedate")]
        public DateTime LastUseDate { get; set; }

        [JsonProperty("createdate")]
        public DateTime CreateDate { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
