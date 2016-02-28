// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Microsoft.Azure.WebJobs.Extensions.Slack
{
    internal class SlackJsonNameContractResolver : DefaultContractResolver
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
                p.PropertyName = rgx.Replace(p.PropertyName, "$1_$2").ToLower(CultureInfo.InvariantCulture);
                return p;
            }).ToList();

            return properties;
        }
    }
}
