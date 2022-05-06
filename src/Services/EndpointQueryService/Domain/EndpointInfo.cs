namespace EndpointQueryService.Domain
{

    public record EndpointInfo : IEntity<int>
    {
        internal const string PathDelimiter = "/";
        public int Id { get; init; }
        public string Path => $"{Account}{PathDelimiter}{Name}{PathDelimiter}{Version}";
        public string Name { get; init; }
        public string Account { get; init; }
        public string Version { get; init; }
        public string IdProperty { get; init; }
        public IEnumerable<string> SearchableBy { get; init; }
        public string SubVersion { get; init; }
    }
}
