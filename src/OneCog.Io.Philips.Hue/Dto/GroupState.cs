﻿using Newtonsoft.Json;

namespace OneCog.Io.Philips.Hue.Dto
{
    public class GroupState
    {
        [JsonProperty("on")]
        public bool On { get; set; }

        [JsonProperty("bri")]
        public int Brightness { get; set; }

        [JsonProperty("hue")]
        public int Hue { get; set; }

        [JsonProperty("sat")]
        public int Saturation { get; set; }

        [JsonProperty("xy")]
        public float[] Xy { get; set; }

        [JsonProperty("ct")]
        public int Ct { get; set; }

        [JsonProperty("effect")]
        public string Effect { get; set; }

        [JsonProperty("colormode")]
        public string ColorMode { get; set; }
    }
}
