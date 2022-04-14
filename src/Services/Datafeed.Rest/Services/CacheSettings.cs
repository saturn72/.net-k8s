namespace Datafeed.Rest.Services
{
    public sealed class CacheSettings
    {
        public sealed class Template
        {
            public const string Prefix = "template:";
            public static TimeSpan Expiration => TimeSpan.FromDays(30);
            public static string BuildGetPathKey(string path) => Prefix + path;
            public static string BuildGetEntriesPageKey(GetAllEntriesContext context)
            {
                var key = BuildGetPathKey(context.Template.Path);
                return $"{key}:offset={context.OffSet}:pagesize={context.PageSize}";
            }
        }
    }
}
