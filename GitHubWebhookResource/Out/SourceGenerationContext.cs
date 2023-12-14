using System.Text.Json.Serialization;

namespace Out;

[JsonSerializable(typeof(ConcoursePayload))]
[JsonSerializable(typeof(List<GetWebhooksResponse>))]
[JsonSerializable(typeof(CreateWebhookPayload))]
public partial class SourceGenerationContext : JsonSerializerContext
{
}