namespace EndpointQueryService.Data
{
    public interface IDbModel<TId>
    {
        public TId Id { get; }
    }
}
