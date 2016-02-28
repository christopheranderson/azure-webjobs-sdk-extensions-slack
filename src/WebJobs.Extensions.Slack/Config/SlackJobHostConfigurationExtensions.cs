// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Azure.WebJobs.Extensions.Slack;

namespace Microsoft.Azure.WebJobs
{
    /// <summary>
    /// Extension methods for Slack integration
    /// </summary>
    public static class SlackJobHostConfigurationExtensions
    {
        /// <summary>
        /// Enables use of the Slack extensions
        /// </summary>
        /// <param name="config">The <see cref="JobHostConfiguration"/> to configure.</param>
        /// <param name="slackConfig">The <see cref="SlackConfiguration"/> to use.</param>
        public static void UseSlack(this JobHostConfiguration config, SlackConfiguration slackConfig = null)
        {
            if (config == null) {
                throw new ArgumentNullException("config");
            }

            if(slackConfig == null)
            {
                slackConfig = new SlackConfiguration();
            }

            config.RegisterExtensionConfigProvider(new SlackExtensionConfig(slackConfig));
        }

        private class SlackExtensionConfig : IExtensionConfigProvider
        {
            private SlackConfiguration _slackConfig;

            public SlackExtensionConfig(SlackConfiguration slackConfig)
            {
                _slackConfig = slackConfig;
            }

            public void Initialize(ExtensionConfigContext context)
            {
                if(context == null)
                {
                    throw new ArgumentNullException("context");
                }

                context.Config.RegisterBindingExtension(new SlackAttributeBindingProvider(_slackConfig, context.Config.NameResolver));
            }
        }
    }
}
