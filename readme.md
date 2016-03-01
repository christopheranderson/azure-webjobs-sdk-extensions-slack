# WebJobs SDK Slack Extension

This extension adds an output binding for sending incoming webhooks to Slack.

```C#
public void SimpleSlackBinding([WebHookTrigger] Message m,
    [Slack(Text = "{Text}", IconEmoji = "{IconEmoji}")] SlackMessage message, 
    TextWriter log)
{
    log.WriteLine(m);
}
```

## Usage

The Slack binding returns a `SlackMessage` object which will then be sent to Slack after the function completes.

You can bind properties from the input object in the usual WebJobs way. If you don't want to bind your Trigger to an object, you can set the `SlackMessage` properties in the function itself.

```C#
public void FullSlackBinding([WebHookTrigger] Message m,
    [Slack(Channel = "{Channel}", 
            IconEmoji = "{IconEmoji}", 
            IsMarkdown = false, 
            Text ="{Text}", 
            Username = "{Username}", 
            WebHookUrl = "{WebHookUrl}")] SlackMessage message,
    TextWriter log
    )
{
    // Further customize Slack Message here. i.e. add attachments, etc.
    // More info on the SlackMessage object on the GitHub project: https://github.com/nerdfury/Slack.Webhooks

    // Continue to set/manipulate properties on the Slack Message, programatically
    message.Text += "WebJobs are Grrrreat!";
    message.IconEmoji = ":troll:";

    // For example, add attachments. See the Slack API docs for more info: https://api.slack.com/docs/attachments
    message.Attachments.Add(new SlackAttachment
    {
        Fallback = "Required plain-text summary of the attachment.",
        Color = "#36a64f",
        Pretext = "Optional text that appears above the attachment block",
        AuthorName = "Bobby Tables",
        AuthorLink = "http://flickr.com/bobby/",
        AuthorIcon = "http://flickr.com/icons/bobby.jpg",
        Title = "Slack API Documentation",
        TitleLink = "https://api.slack.com/",
        Text = "Optional text that appears within the attachment",
        Fields = new List<SlackField>
        {
            new SlackField
            {
                Title = "Priority",
                Value = "High",
                Short = true
            },
            new SlackField
            {
                Title = "Assigned",
                Value = "Bobby",
                Short = true
            }
        },
        ImageUrl = "http://my-website.com/path/to/image.jpg",
        ThumbUrl = "http://example.com/path/to/thumb.png"
    });
}
```

You can enable the Extension via the `JobHostConfiguration` object.
```C#
var config = new JobHostConfiguration();
var slackConfig = new SlackConfiguration();

// These are optional and will be applied if no other value is specified.
slackConfig.WebHookUrl = "";
// IT IS A BAD THING TO HARDCODE YOUR WEBHOOKURL, USE THE APP SETTING "AzureWebJobsSlackWebHookKeyName"
slackConfig.IconEmoji = "";
slackConfig.Username = "";
slackConfig.Channel = "";

config.UseSlack(slackConfig);
```


## Notes

This is currently still in development. Not for production use.

## Tests

Install the `xunit.runner.visualstudio` nuget to run the Tests via Text Explorer

## License

[MIT](LICENSE)