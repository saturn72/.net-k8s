namespace Admin.Backend.Services.Endpoint
{
    public sealed class EndpointCaching
    {
        public const string Prefix = "endpoint:";
        public static TimeSpan Expiration => TimeSpan.FromDays(30);
        public static string BuildGetPathKey(string? path) => Prefix + path?.ToLower();
    }
}
