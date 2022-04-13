namespace Datafeed.Rest.Services.Endpoints
{
    public sealed class AccountEndpointVersionCacheSettings
    {
        private const string AccountEndpointVersionKeyPrefix_Format = "{0}_{1}_{2}";
        public static string AccountEndpointVersionKey(AccountEndpointVersionContext context)
        {
            return string.Format(AccountEndpointVersionKeyPrefix_Format, context.Account.Id, context.Endpoint.Id, context.Version);
        }
        public sealed class GetEntries
        {
            public static TimeSpan Expiration => TimeSpan.FromDays(30);
            public static string GetEntriesPageKey(GetEntriesContext context)
            {
                var p = AccountEndpointVersionKey(context);
                return $"{p}:offset={context.OffSet}:pagesize={context.PageSize}";
            }
        }

    }
}
