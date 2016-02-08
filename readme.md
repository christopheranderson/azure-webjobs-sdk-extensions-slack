# WebJobs SDK Slack Extension

This extension adds an output binding for sending incoming webhooks to Slack.

```
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

```
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
}
```

You can enable the Extension via the `JobHostConfiguration` object.
```
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

## License

[MIT](LICENSE)