namespace Admin.Backend.Domain
{
    public record DatasourceDomainModel
    {
        public string? Id { get; init; }
        public string? Account { get; init; }
        public string? CreatedByUserId { get; init; }
        public string? Name { get; init; }
        public string? Type { get; init; }
    }
}
