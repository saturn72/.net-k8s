using Admin.Backend.Domain;
using Admin.Backend.Services.Datasource;
using Dapper;
using MySql.Data.MySqlClient;

namespace Admin.Backend.Data
{
    public class MySqlDatasourceStore : IDatasourceStore
    {
        private readonly string _connectionString;
        public MySqlDatasourceStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<DatasourceDomainModel?> Create(DatasourceDomainModel datasource)
        {
            using var con = new MySqlConnection(_connectionString);

            var result = await con.ExecuteAsync(Dapper.Datasource.Insert, datasource);

            return result == 1 ?
                await con.QuerySingleOrDefaultAsync<DatasourceDomainModel>(Dapper.Datasource.GetByName, new { datasource.Name }) :
                default;
        }

        public async Task<DatasourceDomainModel?> GetByName(string name)
        {
            using var con = new MySqlConnection(_connectionString);
            return await con.QuerySingleOrDefaultAsync<DatasourceDomainModel>(Dapper.Datasource.GetByName, new { name });
        }

        public async Task<DatasourceDomainModel?> GetDatasourceById(string id)
        {
            using var con = new MySqlConnection(_connectionString);
            return await con.QuerySingleOrDefaultAsync<DatasourceDomainModel>(Dapper.Datasource.GetByName, new { id});
        }
    }
}
