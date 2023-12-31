using System.Text.Json;
using Out;

try
{
    Console.Error.WriteLine("Executing the out...");

    var standardInputPayload = Console.ReadLine();

    Console.Error.WriteLine(standardInputPayload);

    ArgumentException.ThrowIfNullOrEmpty(standardInputPayload);

    var concoursePayload = JsonSerializer.Deserialize(standardInputPayload, SourceGenerationContext.Default.ConcoursePayload);

    ArgumentNullException.ThrowIfNull(concoursePayload);
    ArgumentNullException.ThrowIfNull(concoursePayload.Source);
    ArgumentNullException.ThrowIfNull(concoursePayload.Params);

    ArgumentException.ThrowIfNullOrEmpty(concoursePayload.Source.GitHubBaseUrl);
    ArgumentException.ThrowIfNullOrEmpty(concoursePayload.Source.GitHubToken);

    ArgumentException.ThrowIfNullOrEmpty(concoursePayload.Params.Owner);
    ArgumentException.ThrowIfNullOrEmpty(concoursePayload.Params.Repository);
    ArgumentException.ThrowIfNullOrEmpty(concoursePayload.Params.ResourceName);
    ArgumentException.ThrowIfNullOrEmpty(concoursePayload.Params.WebhookToken);

    var payload = new GitHubWebhookPayload
    {
        GitHubBaseUrl  = concoursePayload.Source.GitHubBaseUrl,
        Token          = concoursePayload.Source.GitHubToken,
        Owner          = concoursePayload.Params.Owner,
        Repository     = concoursePayload.Params.Repository,
        ResourceName   = concoursePayload.Params.ResourceName,
        WebhookToken   = concoursePayload.Params.WebhookToken,
        WebhookBaseUrl = concoursePayload.Params.WebhookBaseUrl,
        Events         = concoursePayload.Params.Events,
        WebhookSecret  = concoursePayload.Params.WebhookSecret,
        InsecureSsl    = concoursePayload.Params.InsecureSsl,
        Pipeline       = concoursePayload.Params.Pipeline
    };

    var client = new HttpClient();
    var service = new GitHubWebhookService(client);

    var webhook = await service.GetMatchingWebhook(payload);

    if (webhook != null)
    {
        if (webhook.Id == null)
        {
            throw new Exception("Could not determine an identifier for the existing webhook.");
        }
        Console.Error.WriteLine("Attempting to update the webhook...");
        await service.UpdateWebhook(payload, webhook.Id.Value);
        Console.Error.WriteLine("Webhook updated successfully.");
    }
    else
    {
        Console.Error.WriteLine("Attempting to create the webhook...");
        await service.CreateWebhook(payload);
        Console.Error.WriteLine("Webhook created successfully.");
    }

    var created = DateTime.UtcNow;

    var output = $"{{\"version\": {{ \"created\":\"{created}\"}}}}";

    Console.WriteLine(output);
}
catch (Exception exception)
{
    Console.Error.WriteLine($"{exception.Message}");
}
