using Admin.Backend.Domain;

namespace Admin.Backend.Data
{
    public sealed partial class Dapper
    {
        public sealed class Endpoint
        {
            private const string TableName = "endpoints";

            public static readonly string Insert = $"INSERT INTO {TableName} " +
                    $"(" +
                    $"{nameof(EndpointDomainModel.Account)}, " +
                    $"CreatedByUserId, " +
                    $"{nameof(EndpointDomainModel.Name)}," +
                    $"{nameof(EndpointDomainModel.Path)}," +
                    $"{nameof(EndpointDomainModel.PathDelimiter)}," +
                    $"{nameof(EndpointDomainModel.Version)}" +
                    $") " +
                    $"VALUES (" +
                    $"@{nameof(EndpointDomainModel.Account)}, " +
                    "@CreatedByUserId, " +
                    $"@{nameof(EndpointDomainModel.Name)}," +
                    $"@{nameof(EndpointDomainModel.Path)}," +
                    $"@{nameof(EndpointDomainModel.PathDelimiter)}," +
                    $"@{nameof(EndpointDomainModel.Version)}" +
                    ");";
            public static readonly string GetIdByPath = $"SELECT {nameof(EndpointDomainModel.Id)} FROM {TableName} WHERE {nameof(EndpointDomainModel.Path)} = @{nameof(EndpointDomainModel.Path)}";

            public static readonly string GetByPath = $"SELECT " +
                $"{nameof(EndpointDomainModel.Id)}, " +
                $"{nameof(EndpointDomainModel.Account)}, " +
                $"{nameof(EndpointDomainModel.CreatedByUserId)}, " +
                $"{nameof(EndpointDomainModel.Name)}, " +
                $"{nameof(EndpointDomainModel.PathDelimiter)}, " +
                $"{nameof(EndpointDomainModel.Path)}, " +
                $"{nameof(EndpointDomainModel.Version)} " +
                $"FROM {TableName} WHERE {nameof(EndpointDomainModel.Path)} = @{nameof(EndpointDomainModel.Path)}";
        }
    }
}
