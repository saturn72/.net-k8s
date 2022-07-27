namespace EndpointQueryService.Domain
{

    public record EndpointMeta
    {
        public DateTime PublishedOnUtc { get; set; }
        public string? IdProperty { get; init; }
        public IEnumerable<string> SearchableBy { get; init; }
        public string? Name { get; init; }
        public string? Account { get; init; }
        public string? Version { get; init; }
        public string? SubVersion { get; init; }
        public int TotalPages { get; init; }
    }
    public record EndpointInfo : IEntity<int>
    {
        internal const string PathDelimiter = "/";
        public int Id { get; init; }
        public string Path => $"{Meta.Account}{PathDelimiter}{Meta.Name}{PathDelimiter}{Meta.Version}";
        public EndpointMeta? Meta { get; init; }
    }
}
