using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebJobs.Extensions.Slack
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class SlackAttribute : Attribute
    {
        public string WebHookUrl { get; set; }

        public string Text { get; set; }

        public string Username { get; set; }

        public string IconEmoji { get; set; }

        public string Channel { get; set; }

        public bool IsMarkdown { get; set; }
    }
}
