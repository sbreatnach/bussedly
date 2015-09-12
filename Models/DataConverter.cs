using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace bussedly.Models
{
    public interface RawDataConverter
    {
        Dictionary<string, dynamic> DictConvert(string rawString);
    }

    public class RawJsonDictConverter : RawDataConverter
    {
        public Dictionary<string, dynamic> DictConvert(string rawString)
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
                    output.Add(pair.Key, DictConvert(pair.Value.ToString()));
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
        private readonly Regex objectRegex 
            = new Regex("^.*?(\x7b.+\x7d).*$");
        private readonly Regex dupeRegex
            = new Regex("(\"direction_extensions\": {.+?},)");
        private readonly RawJsonDictConverter jsonConverter
            = new RawJsonDictConverter();

        public Dictionary<string, dynamic> DictConvert(string rawString)
        {
            var output = new Dictionary<string, dynamic>();
            if (rawString == null || rawString == "")
            {
                return output;
            }

            var match = objectRegex.Match(rawString);
            if (match.Success)
            {
                // remove the invalid data from the matched value e.g. 
                // duplicated keys
                var rawValue = match.Groups[1].Value;
                var dedupedValue = dupeRegex.Replace(rawValue, "");
                output = this.jsonConverter.DictConvert(dedupedValue);
            }
            return output;
        }
    }
}