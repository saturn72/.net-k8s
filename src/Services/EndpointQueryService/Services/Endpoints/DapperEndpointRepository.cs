using Dapper;
using EndpointQueryService.Domain;
using System.Data.SqlClient;

namespace EndpointQueryService.Services.Endpoints
{
    public class DapperEndpointRepository : IEndpointRepository
    {
        private readonly string _connectionString;

        public DapperEndpointRepository(string connectionString)
        {
            _connectionString = connectionString; ;
        }
        public async Task<IEnumerable<EndpointEntry>> GetEntries(GetEntriesContext context)
        {
            var sql = SqlScripts.GetEntriesByVersion(context);

            using var con = new SqlConnection(_connectionString);
            return await con.QueryAsync<EndpointEntry>(sql);
        }

        public async Task<Domain.EndpointInfo> GetEndpointByPath(string path)
        {
            using var con = new SqlConnection(_connectionString);
            return await con.QuerySingleOrDefaultAsync<Domain.EndpointInfo>(SqlScripts.GetTemplateByPath, new { Path = path });
        }

        private sealed class SqlScripts
        {
            private const string SelectFrom = $"SELECT FROM {nameof(Domain.EndpointInfo)}s";
            internal const string GetTemplateByPath = $"{SelectFrom} WHERE [Path] = path";

            internal static string GetEntriesByVersion(GetEntriesContext context)
            {
                var where = $"[{nameof(context.Endpoint.Id)}] = {context.Endpoint.Id} AND " +
                    $"[Version] = {context.Endpoint.Version}";
                return $"{SelectFrom} WHERE {where} " +
                    //$" ORDER BY {orderBy} {sortOrder} " +
                    $"OFFSET {context.OffSet} ROWS " +
              $"FETCH NEXT {context.PageSize} ROWS ONLY;";
            }
        }
    }
}
