namespace Admin.Backend.Services.Account
{
    public sealed class AccountCaching
    {
        public const string Prefix = "account:";
        public static TimeSpan Expiration => TimeSpan.FromDays(30);
        public static string BuildGetNameKey(string name) => $"{Prefix}name-{name.ToLower()}";
    }
}
