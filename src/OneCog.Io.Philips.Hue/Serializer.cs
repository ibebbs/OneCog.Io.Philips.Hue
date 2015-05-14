using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Philips.Hue
{
    public interface ISerializer
    {
        T Deserialize<T>(string json);
        string Serializer<T>(T source);
    }

    public static class Serializer
    {
        private class ForJson : ISerializer
        {
            private static readonly JsonSerializer _serializer = new JsonSerializer();

            public T Deserialize<T>(string json)
            {
                using (StringReader stringReader = new StringReader(json))
                {
                    using (JsonReader jsonReader = new JsonTextReader(stringReader))
                    {
                        return _serializer.Deserialize<T>(jsonReader);
                    }
                }
            }

            public string Serializer<T>(T source)
            {
                StringBuilder builder = new StringBuilder();
             
                using (StringWriter stringWriter = new StringWriter(builder))
                {
                    using (JsonWriter jsonWriter = new JsonTextWriter(stringWriter))
                    {
                        _serializer.Serialize(jsonWriter, source);
                    }
                }

                return builder.ToString();
            }
        }

        public static readonly ISerializer Json = new ForJson();
    }
}
