using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;

using Slack.Webhooks;

namespace WebJobs.Extensions.Slack
{
    internal class SlackAttributeBindingProvider : IBindingProvider
    {
        private readonly SlackConfiguration _config;
        private readonly INameResolver _nameResolver;

        public SlackAttributeBindingProvider(SlackConfiguration config, INameResolver nameResolver)
        {
            _config = config;
            _nameResolver = nameResolver;
        }

        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            ParameterInfo parameter = context.Parameter;
            SlackAttribute attribute = parameter.GetCustomAttribute<SlackAttribute>(inherit: false);
            if(attribute == null)
            {
                return Task.FromResult<IBinding>(null);
            }

            if(context.Parameter.ParameterType != typeof(SlackMessage) && 
                context.Parameter.ParameterType != typeof(SlackMessage).MakeByRefType())
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Can't bind SlackAttribute to type '{0}'.", parameter.ParameterType));
            }

            if(string.IsNullOrEmpty(_config.WebHookUrl))
            {
                throw new InvalidOperationException(
                    string.Format("You must specify a Slack WebHook URL via a '{0}' app setting, via a '{0}' environment variable, or directly in code via SlackConfiguration.WebHookUrl",
                    SlackConfiguration.AzureWebJobsSlackWebHookKeyName));
            }

            return Task.FromResult<IBinding>(new SlackBinding(parameter, attribute, _config, _nameResolver, context));
        }
    }
}
