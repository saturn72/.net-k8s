using EndpointQueryService.Domain;

namespace EndpointQueryService.Services.ActivityLog
{
    public record ApiConsumptionActivityLogRecord
    {
        public DateTimeOffset? PublishedOnUtc { get; set; }
        public EndpointInfo? Endpoint { get; init; }
        public string? Query { get; init; }
        public string? ActivityType { get; init; }
        public string? ConsumedByUserId { get; init; }
        public string? Version { get; init; }
    }
    public sealed class ApiConsumptionActivityType
    {
        internal const string GetMeta = "read.meta";
        internal const string ReadPage = "read.page";
    }
}
