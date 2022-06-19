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

        public async Task Create(CreateContext<DatasourceDomainModel> context)
        {
            if (context?.Model?.Account?.HasValue() == false ||
                context?.Model?.Name?.HasValue() == false || 
                context?.Model?.Type?.HasValue() == false)
            {
                context?.SetErrors($"{nameof(context)}: is null, or have empty/null value for {nameof(context.Model.Account)} and/or {nameof(context.Model.Name)} and/or {nameof(context.Model.Type)}",
                "Bad or missing data");
                return;
            }

            var ds = new
            {
                context.Model.Account,
                context.Model.Name,
                createdByUserId = context.UserId,
                context.Model.Type,
            };
            using var con = new MySqlConnection(_connectionString);
            var result = await con.ExecuteAsync(Dapper.Datasource.Insert, ds);
            if (result == 1)
            {
                context.Response = await con.QuerySingleOrDefaultAsync<string>(Dapper.Datasource.GetIdByAccountAndName, ds);
                return;
            }
            context.Error = "Failed to create new record";
        }

        public async Task<DatasourceDomainModel> GetByName(string name)
        {
            using var con = new MySqlConnection(_connectionString);
            return await con.QuerySingleOrDefaultAsync<DatasourceDomainModel>(Dapper.Datasource.GetByName, new { name });
        }
    }
}
