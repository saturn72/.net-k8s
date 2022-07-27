namespace EndpointQueryService.Data.Endpoints
{
    public record EndpointDbModel : IDbModel<int>
    {
        public int Id { get; init; }
        public string? Account { get; init; }
        public string? Name { get; init; }
        public string? Path { get; init; }
        public string? Version { get; init; }
        public string? SubVersion { get; init; }
    }
}
