namespace EndpointQueryService.Domain
{
    public record EndpointInfo : IEntity<int>
    {
        public int Id { get; init; }
        public string? Path => $"{Account}/{Name}/{Version}";
        public string? Name { get; init; }
        public string? Account { get; init; }
        public string? Version { get; init; }
    }
    public record EndpointEntry
    {
        public int EndpointId { get; init; }
        public string? Version { get; init; }
        public object? Data { get; init; }
        public IEnumerable<KeyValuePair<string, string>>? Tags { get; init; }
    }
}
