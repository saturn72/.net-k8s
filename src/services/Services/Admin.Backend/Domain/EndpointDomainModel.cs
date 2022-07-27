namespace Admin.Backend.Domain
{
    public record EndpointDomainModel
    {
        public string? Id { get; init; }
        public string? Account { get; init; }
        public string? CreatedByUserId { get; init; }
        public string? Name { get; init; }
        public string Path => $"{Account}{PathDelimiter}{Name}{PathDelimiter}{Version}";
        public string? PathDelimiter { get; init; } = Defaults.Endpoint.PathDelimiter;
        public string? Version { get; init; }
    }
}
