﻿using Admin.Backend.Domain;

namespace Admin.Backend.Services.Datasource
{
    public interface IDatasourceStore
    {
        Task<DatasourceDomainModel> Create(DatasourceDomainModel datasource);
        Task<DatasourceDomainModel> GetByName(string name);
        Task<DatasourceDomainModel?> GetDatasourceById(string? id);
        Task<IEnumerable<DatasourceDomainModel>?> GetUserDatasources(string userId,
            string orderBy,
            bool ascOrDesc,
            uint pageSize,
            uint offset);
    }
}
