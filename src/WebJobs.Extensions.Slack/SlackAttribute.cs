// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace Microsoft.Azure.WebJobs
{
    /// <summary>
    /// Attribute used to binds a parameter to a SendGridMessage that will automatically be
    /// sent when the function completes.
    /// </summary>
    /// <remarks>
    /// The method parameter can be of type <see cref="Slack.Webhooks.SlackMessage"/> or a reference
    /// to one ('ref' parameter). When using a reference parameter, you can indicate that the message
    /// should not be sent by setting it to <see langword="null"/> before your job function returns.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class SlackAttribute : Attribute
    {
        /// <summary>
        /// Sets the WebHookUrl for the current outgoing Slack message
        /// </summary>
        /// <remarks>
        /// You can use parameters ("{fromTriggerProperty}") or a name resolver ("%fromINameResolver%")
        /// to format your setting's string.
        /// </remarks>
        public string WebHookUrl { get; set; }

        /// <summary>
        /// Sets the Text for the current outgoing Slack message
        /// </summary>
        /// <remarks>
        /// You can use parameters ("{fromTriggerProperty}") or a name resolver ("%fromINameResolver%")
        /// to format your setting's string.
        /// </remarks>
        public string Text { get; set; }

        /// <summary>
        /// Sets the username for the outgoing request.
        /// </summary>
        /// <remarks>
        /// You can use parameters ("{fromTriggerProperty}") or a name resolver ("%fromINameResolver%")
        /// to format your setting's string.
        /// </remarks>
        public string Username { get; set; }

        /// <summary>
        /// Controls the icon emoji displayed for the message. Use ":{emoji_name}:" in your string.
        /// </summary>
        /// <remarks>
        /// You can use parameters ("{fromTriggerProperty}") or a name resolver ("%fromINameResolver%")
        /// to format your setting's string.
        /// </remarks>
        public string IconEmoji { get; set; }

        /// <summary>
        /// Controls the channel the message is sent to. Use "#{name}" to send to a channel, and "@{name}" to send to a specific user."
        /// </summary>
        /// <remarks>
        /// You can use parameters ("{fromTriggerProperty}") or a name resolver ("%fromINameResolver%")
        /// to format your setting's string.
        /// </remarks>
        public string Channel { get; set; }

        /// <summary>
        /// Tells Slack whether to process this message as Markdown or not
        /// </summary>
        public bool IsMarkdown { get; set; }
    }
}
