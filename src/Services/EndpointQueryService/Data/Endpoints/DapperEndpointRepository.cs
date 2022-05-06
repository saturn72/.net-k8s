using Dapper;
using EndpointQueryService.Domain;
using EndpointQueryService.Services;
using System.Data.SqlClient;

namespace EndpointQueryService.Data.Endpoints
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

        public async Task<EndpointInfo> GetEndpointByPathAsync(string path)
        {
            using var con = new SqlConnection(_connectionString);
            var d = await con.QuerySingleOrDefaultAsync<EndpointDbModel>(SqlScripts.GetTemplateByPath, new { Path = path });
            return ToEndpointInfo(d);
        }
        public static EndpointInfo ToEndpointInfo(EndpointDbModel model) =>
    new()
    {
        Id = model.Id,
        Account = model.Account,
        Name = model.Name,
        Version = model.Version,
    };

        public Task AddOrUpdateEndpointEntryByPath(EndpointInfo info)
        {
            var script = SqlScripts.InsertOrUpdateLatest(info);


            throw new NotImplementedException();
        }

        private sealed class SqlScripts
        {
            private static readonly string Table = $"[{nameof(EndpointInfo)}s]";
            private static readonly string SelectFrom = $"SELECT FROM {Table}";
            internal static readonly string GetTemplateByPath = $"{SelectFrom} WHERE [Path] = path";

            internal static string GetEntriesByVersion(GetEntriesContext context)
            {
                var where = $"[{nameof(context.Endpoint.Id)}] = {context.Endpoint.Id} AND " +
                    $"[Version] = {context.Endpoint.Version}";
                return $"{SelectFrom} WHERE {where} " +
                    //$" ORDER BY {orderBy} {sortOrder} " +
                    $"OFFSET {context.OffSet} ROWS " +
              $"FETCH NEXT {context.PageSize} ROWS ONLY;";
            }

            internal static string InsertOrUpdateLatest(EndpointInfo info)
            {
                return
                "BEGIN TRAN" +
                    $"IF EXISTS (SELECT * FROM {Table} WHERE [{nameof(EndpointInfo.Path)}] = {info.Path}) " +
                        "BEGIN" +
                            $"UPDATE {Table} SET " + //{nameof(EndpointInfo.Data)} = {info.Data} " +
                            $"WHERE [{nameof(EndpointInfo.Path)}] = {info.Path};" +
                        "END " +
                    "ELSE " +
                        "BEGIN" +
                            $"INSERT INTO {Table} ([{nameof(EndpointInfo.Account)}], [{nameof(EndpointInfo.Name)}], [{nameof(EndpointInfo.Version)}]) " +
                            $"VALUES({info.Account}, {info.Name}, {info.Version})" +
                        "END " +
                "COMMIT TRAN";
            }
        }
    }
}
