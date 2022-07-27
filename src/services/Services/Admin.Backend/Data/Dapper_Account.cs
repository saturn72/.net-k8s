using Admin.Backend.Domain;

namespace Admin.Backend.Data
{
    public sealed partial class Dapper
    {
        public sealed class Account
        {
            private const string TableName = "accounts";
            
            private static readonly string SELECT_WHERE = $"SELECT " +
                $"{nameof(AccountDomainModel.Id)}, " +
                $"{nameof(AccountDomainModel.CreatedByUserId)}, " +
                $"{nameof(AccountDomainModel.Name)} " +
                $"FROM {TableName} WHERE ";

            public static readonly string Insert = $"INSERT INTO {TableName} " +
                    $"(" +
                    $"{nameof(AccountDomainModel.CreatedByUserId)}, " +
                    $"{nameof(AccountDomainModel.Name)}" +
                    $") " +
                    $"VALUES (" +
                    $"@{nameof(AccountDomainModel.CreatedByUserId)}, " +
                    $"@{nameof(AccountDomainModel.Name)}" +
                    ");";

            public static readonly string Update = $"UPDATE {TableName} " +
                    "SET " +
                    $"{nameof(AccountDomainModel.Name)} = @{nameof(AccountDomainModel.Name)} " +
                    $"WHERE {nameof(AccountDomainModel.Id)} = @{nameof(AccountDomainModel.Id)};";

            public static readonly string DeleteById = $"DELETE FROM {TableName} WHERE {nameof(AccountDomainModel.Id)} = @{nameof(AccountDomainModel.Id)};";

            public static readonly string GetIdByAccountName = $"SELECT {nameof(AccountDomainModel.Id)} FROM {TableName} WHERE {nameof(AccountDomainModel.Name)} = @{nameof(AccountDomainModel.Name)}";

            public static readonly string GetAccountByAccountName = SELECT_WHERE + $"{nameof(AccountDomainModel.Name)} = @{nameof(AccountDomainModel.Name)}";

            public static readonly string GetAccountBySubjectId = SELECT_WHERE + $"{nameof(AccountDomainModel.CreatedByUserId)} = @{nameof(AccountDomainModel.CreatedByUserId)}";

            public static readonly string GetAccountById = SELECT_WHERE + $"{nameof(AccountDomainModel.Id)} = @{nameof(AccountDomainModel.Id)}";
        }
    }
}
