// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.Azure.WebJobs.Extensions.Tests.Common;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Slack.Webhooks;
using Xunit;

namespace Microsoft.Azure.WebJobs.Extensions.Slack.Tests
{
    public class SlackBindingTests
    {
        [Theory(DisplayName = "Slack - Binding - Creates Expected Message"), MemberData("AttributeAndConfigPermutations")]
        public void CreateDefaultMessage_CreatesExpectedMessage(SlackAttribute attribute, SlackConfiguration config, Dictionary<string, object> bindingData, INameResolver nameResolver,SlackMessage targetMessage, String targetUrl)
        {
            ParameterInfo parameter = GetType().GetMethod("TestMethod", BindingFlags.Static | BindingFlags.NonPublic).GetParameters().First();
            Dictionary<string, Type> contract = new Dictionary<string, Type>
            {
                {"ChannelParam", typeof(string) },
                {"IconParam", typeof(string) },
                {"TextParam", typeof(string) },
                {"UsernameParam", typeof(string) },
                {"WebHookUrlParam", typeof(string) }
            };

            BindingProviderContext context = new BindingProviderContext(parameter, contract, CancellationToken.None);
            SlackBinding binding = new SlackBinding(parameter, attribute, config, nameResolver, context);
            
            // Generate message with input data
            SlackMessage message = binding.CreateDefaultMessage(bindingData);

            // Check that the right values were used to initialize the funtion
            Assert.Equal(targetMessage.Channel, message.Channel);
            Assert.Equal(targetMessage.IconEmoji, message.IconEmoji);
            Assert.Equal(targetMessage.Text, message.Text);
            Assert.Equal(targetMessage.Username, message.Username);
            Assert.Equal(targetMessage.Mrkdwn, message.Mrkdwn);
            Assert.Equal(targetUrl, binding._client.BaseUrl);
        }

        // Private Methods

        private static void TestMethod([Slack] SlackMessage message)
        {
            // no op
        }

        public static IEnumerable<object[]> AttributeAndConfigPermutations
        {
            get
            {
                return new[]
                {
                    new object[] {
                        // Attribute Wins all
                        new SlackAttribute
                        {
                            Channel = "#test-attr",
                            IconEmoji = ":attr:",
                            IsMarkdown = false,
                            Text = "Hello world",
                            Username = "bot-attr",
                            WebHookUrl = "https://hooks.slack.com/services/ATTRIBUTE/FAKE12345/FAKEFAKEFAKEFAKEFAKEFAKE"
                        },
                        new SlackConfiguration
                        {
                            Channel = "#test-config",
                            IconEmoji = ":config:",
                            IsMarkdown = true,
                            Username = "bot-config",
                            WebHookUrl = "https://hooks.slack.com/services/CONFIG123/FAKE12345/FAKEFAKEFAKEFAKEFAKEFAKE"
                        },
                        new Dictionary<string, object>
                        {
                            {"ChannelParam", "#param" },
                            {"IconParam", ":param:" },
                            {"TextParam", "Param" },
                            {"UsernameParam", "Param" },
                            {"WebHookUrlParam", "https://hooks.slack.com/services/PARAM1234/FAKE12345/FAKEFAKEFAKEFAKEFAKEFAKE" }
                        },
                        new TestNameResolver(),
                        new SlackMessage
                        {
                            Channel = "#test-attr",
                            IconEmoji = ":attr:",
                            Mrkdwn = false,
                            Text = "Hello world",
                            Username = "bot-attr",
                        },
                        "https://hooks.slack.com/services/ATTRIBUTE/FAKE12345/FAKEFAKEFAKEFAKEFAKEFAKE"
                    },
                    // Config Wins All
                    new object[] {
                        new SlackAttribute
                        { },
                        new SlackConfiguration
                        {
                            Channel = "#test-config",
                            IconEmoji = ":config:",
                            IsMarkdown = true,
                            Username = "bot-config",
                            WebHookUrl = "https://hooks.slack.com/services/CONFIG123/FAKE12345/FAKEFAKEFAKEFAKEFAKEFAKE"
                        },
                        new Dictionary<string, object>
                        {
                            {"ChannelParam", "#param" },
                            {"IconParam", ":param:" },
                            {"TextParam", "Param" },
                            {"UsernameParam", "Param" },
                            {"WebHookUrlParam", "https://hooks.slack.com/services/PARAM1234/FAKE12345/FAKEFAKEFAKEFAKEFAKEFAKE" }
                        },
                        new TestNameResolver(),
                        new SlackMessage
                        {
                            Channel = "#test-config",
                            IconEmoji = ":config:",
                            Mrkdwn = true,
                            Text = null,
                            Username = "bot-config",
                        },
                        "https://hooks.slack.com/services/CONFIG123/FAKE12345/FAKEFAKEFAKEFAKEFAKEFAKE"
                    },
                    // Param Wins All (except Markdown, which isn't template bound)
                    new object[] {
                        new SlackAttribute
                        {
                            Channel = "{ChannelParam}",
                            IconEmoji = "{IconParam}",
                            Text = "{TextParam}",
                            IsMarkdown = false,
                            Username = "{UsernameParam}",
                            WebHookUrl = "{WebHookUrlParam}"
                        },
                        new SlackConfiguration
                        {
                            Channel = "#test-config",
                            IconEmoji = ":config:",
                            Username = "bot-config",
                            WebHookUrl = "https://hooks.slack.com/services/CONFIG123/FAKE12345/FAKEFAKEFAKEFAKEFAKEFAKE"
                        },
                        new Dictionary<string, object>
                        {
                            {"ChannelParam", "#param" },
                            {"IconParam", ":param:" },
                            {"TextParam", "Param" },
                            {"UsernameParam", "Param" },
                            {"WebHookUrlParam", "https://hooks.slack.com/services/PARAM1234/FAKE12345/FAKEFAKEFAKEFAKEFAKEFAKE" }
                        },
                        new TestNameResolver(),
                        new SlackMessage
                        {
                            Channel = "#param",
                            IconEmoji = ":param:",
                            Mrkdwn = false,
                            Text = "Param",
                            Username = "Param",
                        },
                        "https://hooks.slack.com/services/PARAM1234/FAKE12345/FAKEFAKEFAKEFAKEFAKEFAKE"
                    },
                    // Name Resolver Wins All (except Markdown, which isn't template bound)
                    new object[] {
                        new SlackAttribute
                        {
                            Channel = "%ChannelParam%",
                            IconEmoji = "%IconParam%",
                            Text = "%TextParam%",
                            IsMarkdown = false,
                            Username = "%UsernameParam%",
                            WebHookUrl = "%WebHookUrlParam%"
                        },
                        new SlackConfiguration
                        {
                            Channel = "#test-config",
                            IconEmoji = ":config:",
                            Username = "bot-config",
                            WebHookUrl = "https://hooks.slack.com/services/CONFIG123/FAKE12345/FAKEFAKEFAKEFAKEFAKEFAKE"
                        },
                        new Dictionary<string, object>
                        {
                            {"ChannelParam", "#param" },
                            {"IconParam", ":param:" },
                            {"TextParam", "Param" },
                            {"UsernameParam", "Param" },
                            {"WebHookUrlParam", "https://hooks.slack.com/services/PARAM1234/FAKE12345/FAKEFAKEFAKEFAKEFAKEFAKE" }
                        },
                        new TestNameResolver(),
                        new SlackMessage
                        {
                            Channel = "#nr",
                            IconEmoji = ":nr:",
                            Mrkdwn = false,
                            Text = "NR",
                            Username = "NR",
                        },
                        "https://hooks.slack.com/services/NRNRNRNR1/FAKE12345/FAKEFAKEFAKEFAKEFAKEFAKE"
                    }
                };
            }
        }
    }
}
