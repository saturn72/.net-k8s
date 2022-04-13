namespace Datafeed.Rest.Domain
{
    public record AccountEndpointVersion:IEntity<int>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
