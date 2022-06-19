namespace Admin.Backend.Domain
{
    public record AccountDomainModel
    {
        public string? Id { get; init; }
        public string? CreatedByUserId { get; init; }
        public string? Name { get; init; }
    }
}
