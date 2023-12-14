using System.Text.Json.Serialization;

namespace Out;

public class ConcoursePayload
{
    [JsonPropertyName("params")]
    public Params? Params { get; set; }

    [JsonPropertyName("source")]
    public SourceFields? Source { get; set; }

    [JsonPropertyName("version")]
    public Version? Version { get; set; }
}

public class Params
{
    [JsonPropertyName("repository")]
    public string? Repository { get; set; }

    [JsonPropertyName("owner")]
    public string? Owner { get; set; }

    [JsonPropertyName("webhook_base_url")]
    public string? WebhookBaseUrl { get; set; }

    [JsonPropertyName("resource_name")]
    public string? ResourceName { get; set; }

    [JsonPropertyName("events")]
    public List<string>? Events { get; set; } = new List<string>();

    [JsonPropertyName("webhook_secret")]
    public string? WebhookSecret { get; set; }

    [JsonPropertyName("webhook_token")]
    public string? WebhookToken { get; set; }

    [JsonPropertyName("webhook_token")]
    public string? InsecureSsl { get; set; }

    [JsonPropertyName("pipeline")]
    public string? Pipeline { get; set; }
}

public class SourceFields
{
    [JsonPropertyName("github_base_url")]
    public string? GitHubBaseUrl { get; set; }

    [JsonPropertyName("github_token")]
    public string? GitHubToken { get; set; }
}

public class Version
{
    [JsonPropertyName("value")]
    public string? Value { get; set; }

    [JsonPropertyName("created")]
    public string? Created { get; set; }
}