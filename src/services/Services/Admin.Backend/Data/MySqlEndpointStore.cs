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

        public async Task<EndpointDomainModel?> Create(EndpointDomainModel endpoint)
        {
            using var con = new MySqlConnection(_connectionString);

            var result = await con.ExecuteAsync(Dapper.Endpoint.Insert, endpoint);

            return result == 1 ?
                await con.QuerySingleOrDefaultAsync<EndpointDomainModel>(Dapper.Endpoint.GetByPath, endpoint.Path) :
                default;
        }

        public async Task<EndpointDomainModel> GetByPath(string path)
        {
            using var con = new MySqlConnection(_connectionString);
            return await con.QuerySingleOrDefaultAsync<EndpointDomainModel>(Dapper.Endpoint.GetByPath, new { path });
        }
    }
}
