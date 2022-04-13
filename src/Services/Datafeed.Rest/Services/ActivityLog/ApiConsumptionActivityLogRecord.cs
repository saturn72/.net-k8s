using Datafeed.Rest.Domain;

namespace Datafeed.Rest.Services.ActivityLog
{
    public record ApiConsumptionActivityLogRecord
    {
        public Account? Accout { get; init; }
        public DateTimeOffset? ConsumedOnUtc { get; init; }
        public AccountEndpoint? Endpoint { get; init; }
        public string? Query { get; init; }
        public string? ConsumedByUserId { get; init; }
        public string? Version { get; init; }
    }
}
