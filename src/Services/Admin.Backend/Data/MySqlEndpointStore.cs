using Admin.Backend.Domain;
using Admin.Backend.Services.Endpoint;
using Dapper;
using MySql.Data.MySqlClient;

namespace Admin.Backend.Data
{
    public class MySqlEndpointStore : IEndpointStore
    {
        private readonly string _connectionString;
        public MySqlEndpointStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task Create(CreateContext<EndpointDomainModel> context)
        {
            if (context?.Model?.Name?.HasValue() == false || context?.Model?.Account?.HasValue() == false)
            {
                context?.SetErrors($"{nameof(context)}: is null, or have empty/null value for {nameof(context.Model.Name)} of  {nameof(context.Model.Account)}",
                "Bad or missing data");
                return;
            }

            var ep = new
            {
                context.Model.Account,
                context.Model.Name,
                createdByUserId = context.UserId,
                context.Model.Path,
                context.Model.PathDelimiter,
                context.Model.Version,
            };
            using var con = new MySqlConnection(_connectionString);
            var result = await con.ExecuteAsync(Dapper.Endpoint.Insert, ep);
            if (result == 1)
            {
                context.Response = await con.QuerySingleOrDefaultAsync<string>(Dapper.Endpoint.GetIdByPath, ep);
                return;
            }
            context.Error = "Failed to create new record";
        }

        public async Task<EndpointDomainModel> GetByPath(string path)
        {
            using var con = new MySqlConnection(_connectionString);
            return await con.QuerySingleOrDefaultAsync<EndpointDomainModel>(Dapper.Endpoint.GetByPath, new { path });
        }
    }
}
