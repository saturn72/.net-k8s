namespace EndpointQueryService.Domain
{
    public interface IEntity<TId>
    {
        public TId Id { get; }
    }
}
