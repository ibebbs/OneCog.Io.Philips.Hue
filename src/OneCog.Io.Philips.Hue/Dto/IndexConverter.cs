using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCog.Io.Philips.Hue.Dto
{
    public class IndexConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(T).Equals(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Index<T> index = new Index<T>();

            JObject obj = JObject.Load(reader);

            var items = obj.Properties().SelectMany(
                property =>
                {
                    string name = property.Name;
                    T value = Serializer.Json.Deserialize<T>(property.Value.ToString());

                    int i;

                    if (Int32.TryParse(name, out i) && value != null)
                    {
                        return new Indexed<T>[] { new Indexed<T> { Index = i, Item = value } };
                    }
                    else
                    {
                        return Enumerable.Empty<Indexed<T>>();
                    }
                }
            );

            foreach (Indexed<T> item in items)
            {
                index.Add(item);
            }

            return index;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
