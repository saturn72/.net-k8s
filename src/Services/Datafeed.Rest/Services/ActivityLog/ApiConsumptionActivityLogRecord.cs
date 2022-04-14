using Datafeed.Rest.Domain;

namespace Datafeed.Rest.Services.ActivityLog
{
    public record ApiConsumptionActivityLogRecord
    {
        public DateTimeOffset? PublishedOnUtc { get; set; }
        public Template? Template { get; init; }
        public string? Query { get; init; }
        public string? ConsumedByUserId { get; init; }
        public string? Version { get; init; }
    }
}
