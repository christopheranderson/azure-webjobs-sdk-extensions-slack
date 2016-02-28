// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Slack.Webhooks;


using SlackValueBinder = Microsoft.Azure.WebJobs.Extensions.Slack.SlackBinding.SlackValueBinder;
using RestSharp;
using Microsoft.Azure.WebJobs.Host;

namespace Microsoft.Azure.WebJobs.Extensions.Slack.Tests
{
    public class SlackValueBinderTests
    {
        private readonly SlackValueBinder _valueBinder;
        private readonly SlackMessage _message;

        public SlackValueBinderTests()
        {
            RestClient client = new RestClient("1234");
            _message = new SlackMessage();
            _valueBinder = new SlackValueBinder(client, _message);
        }

        [Fact(DisplayName = "Slack - Value Binder - Returns Expected Type")]
        public void Type_ReturnsExpectedType()
        {
            Assert.Equal(typeof(SlackMessage), _valueBinder.Type);
        }

        [Fact(DisplayName = "Slack - Value Binder - Returns Expected Value")]
        public void GetValue_ReturnsExpectedValue()
        {
            Assert.Same(_message, _valueBinder.GetValue());
        }

        [Fact(DisplayName = "Slack - Value Binder - Invoke String returns null")]
        public void ToInvokeString_ReturnsNull()
        {
            Assert.Null(_valueBinder.ToInvokeString());
        }

        [Fact(DisplayName = "Slack - Value Binder - Set Value throws on invalid url")]
        public async Task SetValueAsync_InvalidUrl_Throws()
        {
            await Assert.ThrowsAsync<FunctionInvocationException>(
                async () => { await _valueBinder.SetValueAsync(_message, CancellationToken.None); }
            );
        }
    }
}
