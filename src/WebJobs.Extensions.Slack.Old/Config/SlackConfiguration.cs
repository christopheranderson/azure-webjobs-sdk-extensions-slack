using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;

namespace WebJobs.Extensions.Slack.Config
{
    public class SlackConfiguration
    {
        internal const string AzureWebJobsSlackWebHookKeyName = "AzureWebJobsSlackWebHookKeyName";

        public string WebHookUrl { get; set; }

        public SlackConfiguration(string WebHookUrl = "")
        {
            if(string.IsNullOrEmpty(WebHookUrl))
            {
                this.WebHookUrl = ConfigurationManager.AppSettings.Get(AzureWebJobsSlackWebHookKeyName)
            }
        }

    }
}
