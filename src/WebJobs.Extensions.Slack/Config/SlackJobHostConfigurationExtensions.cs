using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Config;
using WebJobs.Extensions.Slack;

namespace Microsoft.Azure.WebJobs
{
    public static class SlackJobHostConfigurationExtensions
    {
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
