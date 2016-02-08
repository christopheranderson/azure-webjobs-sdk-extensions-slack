using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.WebHooks;
using WebJobs.Extensions.Slack;
using Slack.Webhooks;
using Sample.models;

namespace Sample
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public void SimpleSlackBinding([WebHookTrigger] Message m,
            [Slack(Text = "{Text}", IconEmoji = "{IconEmoji}")] SlackMessage message, 
            TextWriter log)
        {
            log.WriteLine(m);
        }
    }
}
