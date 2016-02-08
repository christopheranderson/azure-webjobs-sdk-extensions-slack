using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.models
{
    public class Message
    {
        public string WebHookUrl { get; set; }

        public string Text { get; set; }

        public string Username { get; set; }

        public string IconEmoji { get; set; }

        public string Channel { get; set; }

        public bool IsMarkdown { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
