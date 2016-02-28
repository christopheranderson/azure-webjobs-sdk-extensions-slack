// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;
using Microsoft.Azure.WebJobs;
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
            // No code is necessary to send the message. You can customize further here, though.
            log.WriteLine(m);
            /*
            message.Attachments.Add(...)
            */
        }

        public void FullSlackBinding([WebHookTrigger] Message m,
            [Slack(Channel = "{Channel}", 
                   IconEmoji = "{IconEmoji}", 
                   IsMarkdown = false, 
                   Text ="{Text}", 
                   Username = "{Username}", 
                   WebHookUrl = "{WebHookUrl}")] SlackMessage message,
            TextWriter log
            )
        {
            // Further customize Slack Message here. i.e. add attachments, etc.
        }
    }
}
