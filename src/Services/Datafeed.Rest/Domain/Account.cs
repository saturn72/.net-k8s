namespace Datafeed.Rest.Domain
{
    public record Account : IEntity<int>
    {
        public int Id { get; init; }
        public string? Name { get; init; }
    }
}
