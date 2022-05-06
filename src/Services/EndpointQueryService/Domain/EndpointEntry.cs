namespace EndpointQueryService.Domain
{
    public record EndpointEntry
    {
        public int EndpointId { get; init; }
        public string? Version { get; init; }
        public object? Data { get; init; }
        public IEnumerable<KeyValuePair<string, string>>? Tags { get; init; }
    }
}
