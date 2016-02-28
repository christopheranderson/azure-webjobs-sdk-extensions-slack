// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Configuration;

namespace Microsoft.Azure.WebJobs
{
    /// <summary>
    /// Defines the configuration options for the Slack binding.
    /// </summary>
    public class SlackConfiguration
    {
        internal const string AzureWebJobsSlackWebHookKeyName = "AzureWebJobsSlackWebHookKeyName";

        /// <summary>
        /// Gets or sets a global default Slack WebHookURL. If not explicitly set, the value will be defaulted
        /// to the value specified via the 'AzureWebJobsSlackWebHookKeyName' app setting or the
        /// 'AzureWebJobsSlackWebHookKeyName' environment variable.
        /// </summary>
        public string WebHookUrl { get; set; }

        /// <summary>
        /// Gets or sets the default "from: username" that will be used for messages.
        /// This value can be overridden by job functions.
        /// </summary>
        /// <remarks>
        /// This is a useful setting if you're planning on having lots of WebJobs posting
        /// to the same WebHook, and want to differeniate them with separate usernames.
        /// </remarks>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the default "Icon Emoji" that will be used for messages.
        /// This value can be overridden by job functions.
        /// </summary>
        /// <remarks>
        /// This is a useful setting if you're planning on having lots of WebJobs posting
        /// to the same WebHook, and want to differeniate them with separate Icons.
        /// 
        /// The format for emoji is a keyword surrounded by ":". It supports custom emojis.
        /// </remarks>
        /// <example>
        /// Example Icon Emoji for heart: ":heart:"
        /// </example>
        public string IconEmoji { get; set; }

        /// <summary>
        /// Gets or sets the default "to: channel" that will be used for messages.
        /// This value can be overridden by job functions.
        /// </summary>
        /// <remarks>
        /// To send to a specific channel, use the "#channel" pattern.
        /// 
        /// To send to a specific user, use the "@username" pattern.
        /// 
        /// This is a useful setting if you're reusing the same WebHook as another integration, but
        /// want to use a separate channel. It is also helpful for testing purposes.
        /// </remarks>
        /// <example>
        /// Example for sending to a specific channel, bot-channel: "#bot-channel"
        /// Example for sending to a specific user: "@username"
        /// </example>
        public string Channel { get; set; }

        /// <summary>
        /// Gets or sets the default on whether Markdown be used for formatting messages.
        /// This value can be overridden by job functions.
        /// </summary>
        /// <remarks>
        /// This is a useful setting if you're planning on having lots of WebJobs posting
        /// to the same WebHook, and want to differeniate them with separate usernames.
        /// </remarks>
        public bool IsMarkdown { get; set; }

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        public SlackConfiguration(string webHookUrl = null)
        {
            if(string.IsNullOrEmpty(webHookUrl))
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
