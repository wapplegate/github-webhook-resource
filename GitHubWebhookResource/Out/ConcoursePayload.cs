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
    [JsonPropertyName("file")]
    public string? File { get; set; }
}

public class SourceFields
{
    [JsonPropertyName("table_name")]
    public string? TableName { get; set; }

    [JsonPropertyName("key")]
    public string? Key { get; set; }

    [JsonPropertyName("partition_key")]
    public string? PartitionKey { get; set; }

    [JsonPropertyName("access_key")]
    public string? AccessKey { get; set; }

    [JsonPropertyName("secret_key")]
    public string? SecretKey { get; set; }

    [JsonPropertyName("region")]
    public string Region { get; set; } = "us-east-1";
}

public class Version
{
    [JsonPropertyName("value")]
    public string? Value { get; set; }

    [JsonPropertyName("created")]
    public string? Created { get; set; }
}