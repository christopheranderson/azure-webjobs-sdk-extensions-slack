// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Azure.WebJobs;
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
                webhookConfig = new WebHooksConfiguration();
            }

            // These are optional and will be applied if no other value is specified.
            /*
            slackConfig.WebHookUrl = "";
            // IT IS A BAD THING TO HARDCODE YOUR WEBHOOKURL, USE THE APP SETTING "AzureWebJobsSlackWebHookKeyName"
            slackConfig.IconEmoji = "";
            slackConfig.Username = "";
            slackConfig.Channel = "";
            */

            config.UseSlack(slackConfig);
            config.UseWebHooks(webhookConfig);

            var host = new JobHost(config);
            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }
    }
}
