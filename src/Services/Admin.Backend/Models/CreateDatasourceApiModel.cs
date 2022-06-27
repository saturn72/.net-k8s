namespace Admin.Backend.Models
{
    public record CreateDatasourceApiModel
    {
        public string? AccountId { get; init; }
        public string? Name { get; init; }
        public string? Type { get; init; }
    }
}
