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

    ArgumentException.ThrowIfNullOrEmpty(concoursePayload.Params.Repository);
    ArgumentException.ThrowIfNullOrEmpty(concoursePayload.Params.Owner);
    ArgumentException.ThrowIfNullOrEmpty(concoursePayload.Params.WebhookBaseUrl);
    ArgumentException.ThrowIfNullOrEmpty(concoursePayload.Params.ResourceName);
    ArgumentException.ThrowIfNullOrEmpty(concoursePayload.Params.WebhookSecret);
    ArgumentException.ThrowIfNullOrEmpty(concoursePayload.Params.WebhookToken);

    if (concoursePayload.Params.Events == null || !concoursePayload.Params.Events.Any())
    {
        throw new Exception("Events must be provided.");
    }

    ArgumentException.ThrowIfNullOrEmpty(concoursePayload.Source.GitHubBaseUrl);
    ArgumentException.ThrowIfNullOrEmpty(concoursePayload.Source.GitHubToken);

    var payload = new GitHubWebhookPayload
    {
        GitHubBaseUrl  = concoursePayload.Source.GitHubBaseUrl,
        Token          = concoursePayload.Source.GitHubToken,
        Repository     = concoursePayload.Params.Repository,
        Owner          = concoursePayload.Params.Owner,
        WebhookBaseUrl = concoursePayload.Params.WebhookBaseUrl,
        ResourceName   = concoursePayload.Params.ResourceName,
        Events         = concoursePayload.Params.Events,
        WebhookSecret  = concoursePayload.Params.WebhookSecret,
        WebhookToken   = concoursePayload.Params.WebhookToken
    };

    var client = new HttpClient();
    var service = new GitHubWebhookService(client);

    var webhook = await service.DoesWebhookExist(payload);

    if (webhook != null)
    {
        var updateResult = await service.UpdateWebhook(payload, webhook.Id);
    }
    else
    {
        var createResult = await service.CreateWebhook(payload);
    }

    const string value = "testing_value";
    var created = DateTime.UtcNow;

    var output = $"{{\"version\": {{ \"value\": \"{value}\", \"created\":\"{created}\"}}}}";

    Console.WriteLine(output);
}
catch (Exception exception)
{
    Console.Error.WriteLine($"{exception.Message}");
}
