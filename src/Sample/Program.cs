using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using WebJobs.Extensions.Slack;
using Microsoft.Azure.WebJobs.Extensions.WebHooks;

namespace Sample
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var config = new JobHostConfiguration();
            var slackConfig = new SlackConfiguration();
            WebHooksConfiguration webhookConfig;

            if(config.IsDevelopment)
            {
                config.UseDevelopmentSettings();
                webhookConfig = new WebHooksConfiguration(3000);
            }
            else
            {
                webhookConfig = new WebHooksConfiguration(3000);
            }

            // REMOVE THIS
            slackConfig.WebHookUrl = "";
            // IT IS A BAD THING TO HARDCODE YOUR WEBHOOKURL, USE THE APP SETTING "AzureWebJobsSlackWebHookKeyName"
            slackConfig.IconEmoji = "";
            slackConfig.Username = "";
            slackConfig.Channel = "";

            config.UseSlack(slackConfig);
            config.UseWebHooks(webhookConfig);

            var host = new JobHost(config);
            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }
    }
}
