namespace Admin.Backend.Domain
{
    public record AccountDomainModel
    {
        public string? Id { get; init; }
        public string? CreatedByUserId { get; set; }
        public string? Name { get; init; }
    }
}
