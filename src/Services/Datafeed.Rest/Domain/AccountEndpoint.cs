namespace Datafeed.Rest.Domain
{
    public record AccountEndpoint : IEntity<int>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public IEnumerable<string> VersionNames { get; set; } = Array.Empty<string>();
    }
}
