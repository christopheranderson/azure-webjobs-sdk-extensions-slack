using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.RegularExpressions;

namespace WebJobs.Extensions.Slack.Lib
{
    class SlackJsonNameContractResolver : DefaultContractResolver
    {
        private const string pattern = "^(.+)([A-Z])";
        private readonly Regex rgx = new Regex(pattern);

        public SlackJsonNameContractResolver()
        {

        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);

            properties = properties.Select(p => {
                p.PropertyName = rgx.Replace(p.PropertyName, "$1_$2").ToLower();
                return p;
            }).ToList();

            return properties;
        }
    }
}
