using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Bindings.Path;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Slack.Webhooks;

namespace WebJobs.Extensions.Slack.Bindings
{
    internal class SlackBinding : IBinding
    {

        public SlackBinding(ParameterInfo parameter, SlackAttribute attribute, )

        public bool FromAttribute
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Task<IValueProvider> BindAsync(BindingContext context)
        {
            throw new NotImplementedException();
        }

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context)
        {
            throw new NotImplementedException();
        }

        public ParameterDescriptor ToParameterDescriptor()
        {
            throw new NotImplementedException();
        }
    }
}
