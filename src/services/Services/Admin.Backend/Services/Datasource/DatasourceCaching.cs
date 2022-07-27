namespace Admin.Backend.Services.Datasource
{
    public sealed class DatasourceCaching
    {
        public const string Prefix = "datasource:";
        public static TimeSpan Expiration => TimeSpan.FromDays(30);
        public static string BuildGetNameKey(string name) => $"{Prefix}name-{name.ToLower()}";
    }
}
