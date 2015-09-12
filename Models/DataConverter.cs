using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bussedly.Models
{
    public interface RawDataConverter
    {
        object Convert(string rawString);
    }

    public class RawJsonDictConverter : RawDataConverter
    {
        public object Convert(string rawString)
        {
            var output = new Dictionary<string, dynamic>();
            if (rawString == "" || rawString == null)
            {
                return output;
            }

            var rawObj = JsonConvert.DeserializeObject
                <Dictionary<string, dynamic>>(rawString);
            foreach (KeyValuePair<string, dynamic> pair in rawObj)
            {
                if (pair.Value is JObject)
                {
                    output.Add(pair.Key, Convert(pair.Value.ToString()));
                }
                else
                {
                    output.Add(pair.Key, pair.Value);
                }
            }
            return output;
        }
    }

    public class RawJsObjectDataConverter : RawDataConverter
    {
        public object Convert(string rawString)
        {
            // TODO: extract JS object with regex, discard rest, convert JS object
            // with JSON deserializer
            return new Dictionary<string, dynamic>();
        }
    }
}