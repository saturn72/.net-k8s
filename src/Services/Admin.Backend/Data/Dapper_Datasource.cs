﻿using Admin.Backend.Domain;

namespace Admin.Backend.Data
{
    public sealed partial class Dapper
    {
        public sealed class Datasource
        {
            private const string TableName = "datasources";

            public static readonly string Insert = $"INSERT INTO {TableName} " +
                    $"(" +
                    $"{nameof(DatasourceDomainModel.CreatedByUserId)}," +
                    $"{nameof(DatasourceDomainModel.Name)}," +
                    $"{nameof(DatasourceDomainModel.Type)}" +
                    $") " +
                    $"VALUES (" +
                    $"@{nameof(DatasourceDomainModel.CreatedByUserId)}, " +
                    $"@{nameof(DatasourceDomainModel.Name)}," +
                    $"@{nameof(DatasourceDomainModel.Type)}" +
                    ");";

            public static readonly string GetIdByAccountAndName = $"SELECT {nameof(DatasourceDomainModel.Id)} FROM {TableName} WHERE {nameof(DatasourceDomainModel.Account)} = @{nameof(DatasourceDomainModel.Account)} AND {nameof(DatasourceDomainModel.Name)} = @{nameof(DatasourceDomainModel.Name)}";

            public static readonly string GetByName = $"SELECT " +
                $"{nameof(DatasourceDomainModel.Id)}, " +
                $"{nameof(DatasourceDomainModel.CreatedByUserId)}, " +
                $"{nameof(DatasourceDomainModel.Name)}, " +
                $"{nameof(DatasourceDomainModel.Type)} " +
                $"FROM {TableName} WHERE {nameof(DatasourceDomainModel.Name)} = @{nameof(DatasourceDomainModel.Name)} AND "+
                $"{nameof(DatasourceDomainModel.CreatedByUserId)} = @{nameof(DatasourceDomainModel.CreatedByUserId)}";
        }
    }
}
