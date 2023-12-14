using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Out;

public class GitHubWebhookService
{
    private readonly HttpClient client;

    public GitHubWebhookService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<bool> CreateWebhook(GitHubWebhookPayload payload)
    {
        var uri = new Uri($"{payload.GitHubBaseUrl}/repos/{payload.Owner}/{payload.Repository}/hooks");

        var webhookUrl = GenerateWebhookUrl(payload);

        var requestBody = new CreateWebhookPayload
        {
            Name  = "web",
            Active = true,
            Events = payload.Events,
            Config = new CreateWebhookConfig
            {
                ContentType = "json",
                InsecureSsl = string.IsNullOrWhiteSpace(payload.InsecureSsl) ? "0" : payload.InsecureSsl,
                Secret      = payload.WebhookSecret,
                Url         = webhookUrl
            }
        };

        var message = new HttpRequestMessage();
        message.RequestUri = uri;
        message.Method = HttpMethod.Post;

        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", payload.Token);
        message.Headers.Add("User-Agent", "Concourse");
        message.Headers.Add("Accept", "application/vnd.github+json");
        message.Headers.Add("X-GitHub-Api-Version", "2022-11-28");

        message.Content = new StringContent(JsonSerializer.Serialize(requestBody, SourceGenerationContext.Default.CreateWebhookPayload));

        var response = await client.SendAsync(message);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Could not create webhook.");
        }

        var body = await response.Content.ReadAsStringAsync();

        return true;
    }

    public async Task<bool> UpdateWebhook(GitHubWebhookPayload payload, int hookIdentifier)
    {
        var uri = new Uri($"{payload.GitHubBaseUrl}/repos/{payload.Owner}/{payload.Repository}/hooks/{hookIdentifier}");

        var webhookUrl = GenerateWebhookUrl(payload);

        var requestBody = new CreateWebhookPayload
        {
            Events = payload.Events,
            Config = new CreateWebhookConfig
            {
                ContentType = "json",
                InsecureSsl = string.IsNullOrWhiteSpace(payload.InsecureSsl) ? "0" : payload.InsecureSsl,
                Secret      = payload.WebhookSecret,
                Url         = webhookUrl
            }
        };

        var message = new HttpRequestMessage();
        message.RequestUri = uri;
        message.Method = HttpMethod.Patch;

        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", payload.Token);
        message.Headers.Add("User-Agent", "Concourse");
        message.Headers.Add("Accept", "application/vnd.github+json");
        message.Headers.Add("X-GitHub-Api-Version", "2022-11-28");

        message.Content = new StringContent(JsonSerializer.Serialize(requestBody, SourceGenerationContext.Default.CreateWebhookPayload));

        var response = await client.SendAsync(message);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Could not create webhook.");
        }

        var body = await response.Content.ReadAsStringAsync();

        return true;
    }

    public async Task<List<GetWebhooksResponse>> GetWebhooks(GitHubWebhookPayload payload)
    {
        var uri = new Uri($"{payload.GitHubBaseUrl}/repos/{payload.Owner}/{payload.Repository}/hooks");

        var message = new HttpRequestMessage();
        message.RequestUri = uri;
        message.Method = HttpMethod.Get;

        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", payload.Token);
        message.Headers.Add("User-Agent", "Concourse");
        message.Headers.Add("Accept", "application/vnd.github+json");
        message.Headers.Add("X-GitHub-Api-Version", "2022-11-28");

        var response = await client.SendAsync(message);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("The request to retrieve repository webhooks has failed.");
        }

        var body = await response.Content.ReadAsStringAsync();
        var webhooks = JsonSerializer.Deserialize(body, SourceGenerationContext.Default.ListGetWebhooksResponse);

        if (webhooks == null)
        {
            throw new Exception("An error occurred.");
        }

        return webhooks;
    }

    public async Task<GetWebhooksResponse?> GetMatchingWebhook(GitHubWebhookPayload payload)
    {
        var url = GenerateWebhookUrl(payload);

        var webhooks = await GetWebhooks(payload);

        foreach (var webhook in webhooks)
        {
            if (webhook.Config.Url == url)
            {
                return webhook;
            }
        }

        return null;
    }

    private static string GenerateWebhookUrl(GitHubWebhookPayload payload)
    {
        var atcExternalUrl = Environment.GetEnvironmentVariable("ATC_EXTERNAL_URL");
        var buildTeamName = Environment.GetEnvironmentVariable("BUILD_TEAM_NAME");
        var buildPipelineName = Environment.GetEnvironmentVariable("BUILD_PIPELINE_NAME");

        var baseUrl = string.IsNullOrWhiteSpace(payload.WebhookBaseUrl) ? atcExternalUrl : payload.WebhookBaseUrl;

        var webhookUrl = $"{baseUrl}/api/v1/teams/{buildTeamName}/pipelines/{buildPipelineName}/resources/{payload.ResourceName}/check/webhook?webhook_token={payload.WebhookToken}";

        return Uri.EscapeDataString(webhookUrl);
    }
}

public class GitHubWebhookPayload
{
    public string? GitHubBaseUrl { get; set; }

    public string? Repository { get; set; }

    public string? Owner { get; set; }

    public string? Token { get; set; }

    public string? WebhookBaseUrl { get; set; }

    public string? ResourceName { get; set; }

    public List<string>? Events { get; set; }

    public string? WebhookSecret { get; set; }

    public string? WebhookToken { get; set; }

    public string? InsecureSsl { get; set; }
}

public class GetWebhooksResponse
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("test_url")]
    public string? TestUrl { get; set; }

    [JsonPropertyName("ping_url")]
    public string? PingUrl { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("events")]
    public List<string> Events { get; set; } = new List<string>();

    [JsonPropertyName("active")]
    public bool Active { get; set; }

    [JsonPropertyName("config")]
    public GetWebhooksConfig? Config { get; set; }

    [JsonPropertyName("updated_at")]
    public string? UpdatedAt { get; set; }

    [JsonPropertyName("created_at")]
    public string? CreatedAt { get; set; }
}

public class GetWebhooksConfig
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("content_type")]
    public string? ContentType { get; set; }

    [JsonPropertyName("insecure_ssl")]
    public string? InsecureSsl { get; set; }
}

public class CreateWebhookPayload
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("active")]
    public bool Active { get; set; }

    [JsonPropertyName("events")]
    public List<string>? Events { get; set; }

    [JsonPropertyName("config")]
    public CreateWebhookConfig? Config { get; set; }
}

public class CreateWebhookConfig
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("content_type")]
    public string? ContentType { get; set; }

    [JsonPropertyName("secret")]
    public string? Secret { get; set; }

    [JsonPropertyName("insecure_ssl")]
    public string? InsecureSsl { get; set; }
}