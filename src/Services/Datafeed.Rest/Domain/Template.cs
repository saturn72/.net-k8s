namespace Datafeed.Rest.Domain
{
    public record Template : IEntity<int>
    {
        public int Id { get; init; }
        public string? Path => $"{Account}/{Endpoint}/{Name}/{Version}";
        public string? Name { get; init; }
        public string? Account { get; init; }
        public string? Endpoint { get; init; }
        public string? Version { get; init; }
    }
    public record TemplateEntry
    {
        public int TemplateId { get; init; }
        public string? Version { get; init; }
        public object? Data { get; init; }
    }
}
