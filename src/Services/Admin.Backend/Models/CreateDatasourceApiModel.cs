namespace Admin.Backend.Models
{
    public record CreateDatasourceApiModel
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
    }
}
