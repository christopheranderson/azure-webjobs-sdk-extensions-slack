// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;
using Microsoft.Azure.WebJobs;
using Slack.Webhooks;
using Sample.models;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Net.Http.Formatting;
using Microsoft.AspNet.WebHooks;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections.Generic;

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
            // More info on the SlackMessage object on the GitHub project: https://github.com/nerdfury/Slack.Webhooks

            // Continue to set/manipulate properties on the Slack Message, programatically
            message.Text += "WebJobs are Grrrreat!";
            message.IconEmoji = ":troll:";

            // For example, add attachments. See the Slack API docs for more info: https://api.slack.com/docs/attachments
            message.Attachments.Add(new SlackAttachment
            {
                Fallback = "Required plain-text summary of the attachment.",
                Color = "#36a64f",
                Pretext = "Optional text that appears above the attachment block",
                AuthorName = "Bobby Tables",
                AuthorLink = "http://flickr.com/bobby/",
                AuthorIcon = "http://flickr.com/icons/bobby.jpg",
                Title = "Slack API Documentation",
                TitleLink = "https://api.slack.com/",
                Text = "Optional text that appears within the attachment",
                Fields = new List<SlackField>
                {
                    new SlackField
                    {
                        Title = "Priority",
                        Value = "High",
                        Short = true
                    },
                    new SlackField
                    {
                        Title = "Assigned",
                        Value = "Bobby",
                        Short = true
                    }
                },
                ImageUrl = "http://my-website.com/path/to/image.jpg",
                ThumbUrl = "http://example.com/path/to/thumb.png"
            });
        }

        // Workflow via Slack
        public async Task SlackIniatedWebHook([WebHookTrigger("slack/webhook")] WebHookContext context,
            [Queue("SlackWork")] ICollector<SlackWork> messages
        )
        {
            // Try and parse the Slack Message Body with simple helper method
            NameValueCollection nvc;
            if(TryParseSlackBody(await context.Request.Content.ReadAsStringAsync(), out nvc))
            {
                Regex rgx = new Regex("(\\d+) (\\d+)");
                Match match = rgx.Match(nvc["text"]);
                int count;
                int work;
                if(int.TryParse(match.Groups[1].Value, out count) && int.TryParse(match.Groups[2].Value, out work))
                {
                    for (int i = 0; i < count; i++)
                    {
                        messages.Add(new SlackWork { id = i, work = work, replyUrl = nvc["response_url"], username = nvc["user_name"] });
                    }

                    // All good, quickly send an affirmative response
                    context.Response = new HttpResponseMessage(HttpStatusCode.Accepted)
                    {
                        Content = new StringContent("Message received! Processing ...")
                    };
                }
                else
                {
                    // Not good, quick send a negative response
                    context.Response = new HttpResponseMessage(HttpStatusCode.Accepted)
                    {
                        Content = new StringContent("Incorrect format - please pass two numbers along - i.e. /cmd 2 30")
                    };

                    // We can stop here for the failure case
                    return;
                }
            }
            else
            {
                // Not good, quick send a negative response
                context.Response = new HttpResponseMessage(HttpStatusCode.Accepted)
                {
                    Content = new StringContent("Something went wrong. :(")
                };

                // We can stop here for the failure case
                return;
            }

            
        }

        public void ProcessSlackWork([QueueTrigger("SlackWork")] SlackWork work, 
            [Slack(WebHookUrl = "{replyUrl}", Text = "Item: {id} finished processing {work} seconds of work.", IconEmoji = ":sleepy:", Channel = "@{username}")] SlackMessage slack,
            TextWriter log
        )
        {
            log.WriteLine($"Processing id: {work.id} - working for {work.work} seconds");


            if(work.work > 10)
            {
                slack.IconEmoji = ":tada:";
            }

            int sleepFor = (work.work + (int)(.2 * work.work * (new Random()).NextDouble())) * 1000;
            log.WriteLine($"Processing id: {work.id} - actually working for {sleepFor} seconds, because of some made up, factor of error");
            Thread.Sleep(sleepFor);
        }

        private bool TryParseSlackBody(string body, out NameValueCollection nvc)
        {
            body = body.Replace('\n', '&');
            body = body.Replace("\r", "");
            nvc = System.Web.HttpUtility.ParseQueryString(body);

            return nvc.Count > 0;
        }

        
    }

    public class SlackWork
    {
        public int id { get; set; }
        public string username { get; set; }
        public int work { get; set; }
        public string replyUrl { get; set; }
    }

}
