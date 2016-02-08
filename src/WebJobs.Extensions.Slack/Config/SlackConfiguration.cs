using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;

namespace WebJobs.Extensions.Slack
{
    public class SlackConfiguration
    {
        internal const string AzureWebJobsSlackWebHookKeyName = "AzureWebJobsSlackWebHookKeyName";

        public string WebHookUrl { get; set; }

        public string Username { get; set; }

        public string IconEmoji { get; set; }

        public string Channel { get; set; }

        public bool IsMarkdown { get; set; }

        public SlackConfiguration(string WebHookUrl = "")
        {
            if(string.IsNullOrEmpty(WebHookUrl))
            {
                this.WebHookUrl = ConfigurationManager.AppSettings.Get(AzureWebJobsSlackWebHookKeyName);
                if(string.IsNullOrEmpty(this.WebHookUrl))
                {
                    this.WebHookUrl = Environment.GetEnvironmentVariable(AzureWebJobsSlackWebHookKeyName);
                }
            }
        }

    }
}
