namespace Admin.Backend.Services.Account
{
    public sealed class AccountCaching
    {
        public const string Prefix = "account:";
        internal static TimeSpan Expiration => TimeSpan.FromDays(30);
        internal static TimeSpan DeleteTokenExpiration => TimeSpan.FromSeconds(90);
        internal static string BuildGetByNameKey(string name) => $"{Prefix}name-{name.ToLower()}";
        internal static string BuildGetBySubjectIdKey(string subjectId) => $"{Prefix}subjectId-{subjectId}";

        internal static string BuildDeletionToken(string subjectId, string accountId) => $"{Prefix}delete:subjectId-{subjectId.ToLower()}|accountId-{accountId}";
    }
}
