namespace EndpointQueryService.Services
{
    public sealed class CacheSettings
    {
        public sealed class Endpoint
        {
            public const string Prefix = "endpoint:";
            public static TimeSpan Expiration => TimeSpan.FromDays(30);
            public static string BuildGetPathKey(string path) => Prefix + path;
            public static string BuildGetEntriesPageKey(GetEntriesContext context)
            {
                var key = BuildGetPathKey(context.Endpoint.Path);
                return $"{key}:offset={context.OffSet}:pagesize={context.PageSize}";
            }
        }
    }
}
