using Newtonsoft.Json;
using System;

namespace OneCog.Io.Philips.Hue.Dto
{
    public class Schedule
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("command")]
        public Command Command { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }
    }
}
