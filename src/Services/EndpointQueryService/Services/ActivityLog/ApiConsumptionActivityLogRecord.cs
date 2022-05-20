using EndpointQueryService.Domain;

namespace EndpointQueryService.Services.ActivityLog
{
    public record ApiConsumptionActivityLogRecord
    {
        public DateTimeOffset? PublishedOnUtc { get; set; }
        public Domain.EndpointInfo? Endpoint { get; init; }
        public string? Query { get; init; }
        public string? ConsumedByUserId { get; init; }
        public string? Version { get; init; }
    }
}
