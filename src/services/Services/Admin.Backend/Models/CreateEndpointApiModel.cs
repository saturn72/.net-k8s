namespace Admin.Backend.Models
{
    public record CreateEndpointApiModel
    {
        public string? Name { get; set; }
        public string? Account { get; set; }
        public string? Version { get; set; }
    }
}
