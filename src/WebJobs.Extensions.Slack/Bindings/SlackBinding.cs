// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Bindings.Path;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Slack.Webhooks;
using System.Reflection;
using Microsoft.Azure.WebJobs;
using System.Threading;
using Newtonsoft.Json;
using RestSharp;

namespace Microsoft.Azure.WebJobs.Extensions.Slack
{
    internal class SlackBinding : IBinding
    {
        private readonly ParameterInfo _parameter;
        private readonly SlackAttribute _attribute;
        private readonly SlackConfiguration _config;
        private readonly INameResolver _nameResolver;
        internal RestClient _client;
        private readonly BindingTemplate _webHookUrlBindingTemplate;
        private readonly BindingTemplate _textBindingTemplate;
        private readonly BindingTemplate _usernameBindingTemplate;
        private readonly BindingTemplate _iconEmojiBindingTemplate;
        private readonly BindingTemplate _channelBindingTemplate;

        public SlackBinding(ParameterInfo parameter, SlackAttribute attribute, SlackConfiguration config, INameResolver nameResolver, BindingProviderContext context)
        {
            _parameter = parameter;
            _attribute = attribute;
            _config = config;
            _nameResolver = nameResolver;
            
            if(!string.IsNullOrEmpty(_attribute.WebHookUrl))
            {
                _webHookUrlBindingTemplate = CreateBindingTemplate(_attribute.WebHookUrl, context.BindingDataContract);
            }

            if (!string.IsNullOrEmpty(_attribute.Text))
            {
                _textBindingTemplate = CreateBindingTemplate(_attribute.Text, context.BindingDataContract);
            }

            if (!string.IsNullOrEmpty(_attribute.Username))
            {
                _usernameBindingTemplate = CreateBindingTemplate(_attribute.Username, context.BindingDataContract);
            }

            if (!string.IsNullOrEmpty(_attribute.IconEmoji))
            {
                _iconEmojiBindingTemplate = CreateBindingTemplate(_attribute.IconEmoji, context.BindingDataContract);
            }

            if (!string.IsNullOrEmpty(_attribute.Channel))
            {
                _channelBindingTemplate = CreateBindingTemplate(_attribute.Channel, context.BindingDataContract);
            }
        }

        public bool FromAttribute
        {
            get
            {
                return true;
            }
        }

        public async Task<Microsoft.Azure.WebJobs.Host.Bindings.IValueProvider> BindAsync(BindingContext context)
        {
            SlackMessage message = CreateDefaultMessage(context.BindingData);

            return await BindAsync(message, context.ValueContext);
        }

        public Task<Microsoft.Azure.WebJobs.Host.Bindings.IValueProvider> BindAsync(object value, ValueBindingContext context)
        {
            SlackMessage message = (SlackMessage)value;

            return Task.FromResult<Microsoft.Azure.WebJobs.Host.Bindings.IValueProvider>(new SlackValueBinder(_client, message));
        }

        public ParameterDescriptor ToParameterDescriptor()
        {
            return new ParameterDescriptor
            {
                Name = _parameter.Name
            };
        }

        public SlackMessage CreateDefaultMessage(IReadOnlyDictionary<string, object> bindingData)
        {
            SlackMessage message = new SlackMessage();

            // Set message
            if(_textBindingTemplate != null)
            {
                message.Text = _textBindingTemplate.Bind(bindingData);
            }

            // Set channel
            if(_channelBindingTemplate != null)
            {
                message.Channel = _channelBindingTemplate.Bind(bindingData);
            }
            else if(!string.IsNullOrEmpty(_config.Channel))
            {
                message.Channel = _config.Channel;
            }

            // Set icon emoji
            if(_iconEmojiBindingTemplate != null)
            {
                message.IconEmoji = _iconEmojiBindingTemplate.Bind(bindingData);
            }
            else if(!string.IsNullOrEmpty(_config.IconEmoji))
            {
                message.IconEmoji = _config.IconEmoji;
            }

            // Set markdown
            if (_iconEmojiBindingTemplate != null)
            {
                message.Mrkdwn = _attribute.IsMarkdown;
            }
            else if (!string.IsNullOrEmpty(_config.IconEmoji))
            {
                message.Mrkdwn = _config.IsMarkdown;
            }

            // Set username
            if (_usernameBindingTemplate != null)
            {
                message.Username = _usernameBindingTemplate.Bind(bindingData);
            }
            else if(!string.IsNullOrEmpty(_config.Username))
            {
                message.Username = _config.Username;
            }

            if(_webHookUrlBindingTemplate != null)
            {
                _client = new RestClient(_webHookUrlBindingTemplate.Bind(bindingData));
            }
            else if(!string.IsNullOrEmpty(_config.WebHookUrl))
            {
                _client = new RestClient(_config.WebHookUrl);
            }

            return message;
        }

        private BindingTemplate CreateBindingTemplate(string pattern, IReadOnlyDictionary<string, Type> bindingDataContract)
        {
            if (_nameResolver != null)
            {
                pattern = _nameResolver.ResolveWholeString(pattern);
            }
            BindingTemplate bindingTemplate = BindingTemplate.FromString(pattern);
            bindingTemplate.ValidateContractCompatibility(bindingDataContract);

            return bindingTemplate;
        }

        internal class SlackValueBinder : IValueBinder
        {
            private readonly RestClient _client;
            private readonly SlackMessage _message;

            public SlackValueBinder(RestClient client, SlackMessage message)
            {
                _client = client;
                _message = message;
            }

            public Type Type
            {
                get
                {
                    return typeof(SlackMessage);
                }
            }

            public object GetValue()
            {
                return _message;
            }

            public async Task SetValueAsync(object value, CancellationToken cancellationToken)
            {
                if(value == null)
                {
                    return;
                }

                var results = await ExecuteRequest(_client, _message, cancellationToken);

                if(results.ResponseStatus != ResponseStatus.Completed)
                {
                    throw new FunctionInvocationException("Slack Message could not be delivered. See Exception details for more information.", results.ErrorException);
                }
            }

            public string ToInvokeString()
            {
                return _message.Text;
            }

            private async Task<IRestResponse> ExecuteRequest(RestClient client, SlackMessage message, CancellationToken cancellationToken)
            {
                RestRequest req = new RestRequest(Method.POST);
                req.RequestFormat = DataFormat.Json;
                req.AddBody(message);
                var config = new JsonSerializerSettings();
                config.NullValueHandling = NullValueHandling.Ignore;
                config.ContractResolver = new SlackJsonNameContractResolver();
                req.Parameters[0].Value = JsonConvert.SerializeObject(message, config);
                return await Task.Run(
                    () => {
                        var results = client.Execute(req);
                        return results;
                    }, 
                    cancellationToken);
            }
        }
    }
}
