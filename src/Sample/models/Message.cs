// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


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
